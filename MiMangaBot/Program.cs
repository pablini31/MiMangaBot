using MiMangaBot.Services.Features.Mangas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiMangaBot API V1");
        c.RoutePrefix = "swagger";
    });
}

// Importante: el orden de estos middleware es crucial
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

// Mapear los controladores
app.MapControllers();

// Endpoint de prueba simple
app.MapGet("/", () => "¡API de Manga funcionando!");

// Endpoint de prueba adicional
app.MapGet("/test", () => new { 
    status = "success", 
    message = "Test endpoint funcionando" 
});

app.Run();
