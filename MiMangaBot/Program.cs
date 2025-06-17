using Microsoft.OpenApi.Models;
using MiMangaBot.Services.Features.Mangas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

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

// Habilitar Swagger SIEMPRE
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiMangaBot API V1");
    c.RoutePrefix = string.Empty; // Swagger en la raíz
});

//app.UseHttpsRedirection(); // Eliminado para evitar problemas de redirección
app.UseAuthorization();
app.MapControllers();

app.Run();
