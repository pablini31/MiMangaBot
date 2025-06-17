using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MiMangaBot.Domain.Data;
using MiMangaBot.Services.Features.Mangas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configurar la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "MiMangaBot API", 
        Version = "v1",
        Description = "API para gestionar mangas"
    });
});

// Registrar el MangaService
builder.Services.AddScoped<MangaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiMangaBot API V1");
        c.RoutePrefix = "swagger";
    });
}

// Habilitar CORS
app.UseCors();

//app.UseHttpsRedirection(); // Eliminado para evitar problemas de redirección
app.UseAuthorization();
app.MapControllers();

app.Run();
