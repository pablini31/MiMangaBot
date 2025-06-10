using Microsoft.EntityFrameworkCore;
using MiMangaBot.Domain.Data;
using MiMangaBot.Services.Features.Mangas;
using MiMangaBot.Scripts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configurar la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Registrar el seeder
builder.Services.AddScoped<MangaDataSeeder>();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MiMangaBot API",
        Version = "v1",
        Description = "API para gestionar una colección de mangas"
    });
});

// Registrar el MangaService
builder.Services.AddScoped<MangaService>();

// Add CORS
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

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiMangaBot API V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Agregar un endpoint de prueba
app.MapGet("/", () => "API is running! Go to /swagger for the API documentation");

Console.WriteLine("Application is starting...");
Console.WriteLine("Swagger UI should be available at: http://localhost:5000/swagger");

app.Run();
