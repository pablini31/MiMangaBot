using Microsoft.EntityFrameworkCore;
using MiMangaBot.Domain.Data;
using Microsoft.OpenApi.Models;
using MiMangaBot.Services.Features.Mangas;
using MiMangaBot.Domain.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure DbContext with retry policy and optimized settings
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
            sqlOptions.CommandTimeout(60);
        });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Configure Swagger with enhanced documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "MiMangaBot API", 
        Version = "v1.0.0",
        Description = "API completa para gestionar mangas y géneros con información detallada de relaciones.",
        Contact = new OpenApiContact
        {
            Name = "MiMangaBot Team",
            Email = "support@mimangabot.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Include XML comments for better documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Add operation filters for better organization
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        var controllerActionDescriptor = api.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
        if (controllerActionDescriptor != null)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }

        return new[] { api.RelativePath };
    });

    // Add security definitions if needed
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
});

// Registrar los servicios
builder.Services.AddScoped<MangaService>();
builder.Services.AddScoped<MangaSeederService>();
builder.Services.AddScoped<IMangaRepository, MangaRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseStaticFiles();

// Enhanced Swagger UI configuration
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiMangaBot API v1.0.0");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "MiMangaBot API Documentation";
    c.DefaultModelsExpandDepth(2);
    c.DefaultModelExpandDepth(2);
    c.DisplayRequestDuration();
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    c.EnableDeepLinking();
    c.EnableFilter();
    c.ShowExtensions();
    c.ShowCommonExtensions();
});

// Use CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Verificar la conexión a la base de datos al iniciar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var canConnect = await context.Database.CanConnectAsync();
        if (canConnect)
        {
            app.Logger.LogInformation("✅ Conexión a la base de datos establecida correctamente.");
            
            // Verificar si la tabla Mangas existe
            var tableExists = await context.Database.ExecuteSqlRawAsync(
                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Mangas'") > 0;
            
            if (tableExists)
            {
                app.Logger.LogInformation("✅ Tabla 'Mangas' encontrada en la base de datos.");
            }
            else
            {
                app.Logger.LogWarning("⚠️ Tabla 'Mangas' no encontrada en la base de datos.");
            }
        }
        else
        {
            app.Logger.LogError("❌ No se pudo establecer conexión con la base de datos.");
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "❌ Error al verificar la conexión con la base de datos: {Message}", ex.Message);
    }
}

app.Run();
