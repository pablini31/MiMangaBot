using Microsoft.EntityFrameworkCore;
using MiMangaBot.Domain.Data;
using MiMangaBot.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MiMangaBot.Scripts
{
    public class MangaDataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MangaDataSeeder> _logger;
        private readonly string[] _estados = { "En emisión", "Finalizado", "Hiatus", "Cancelado" };
        private readonly string[] _generos = { "Acción", "Aventura", "Comedia", "Drama", "Fantasía", "Horror", "Misterio", "Romance", "Ciencia Ficción", "Slice of Life" };
        private readonly Random _random = new Random();

        public MangaDataSeeder(ApplicationDbContext context, ILogger<MangaDataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedMangasAsync(int count)
        {
            try
            {
                _logger.LogInformation($"Iniciando inserción de {count} mangas...");
                
                // Verificar si ya existen mangas
                var existingCount = await _context.Mangas.CountAsync();
                _logger.LogInformation($"Mangas existentes en la base de datos: {existingCount}");

                if (existingCount > 0)
                {
                    _logger.LogInformation("La base de datos ya contiene mangas. Limpiando...");
                    _context.Mangas.RemoveRange(await _context.Mangas.ToListAsync());
                    await _context.SaveChangesAsync();
                }

                var mangas = new List<Manga>();
                for (int i = 1; i <= count; i++)
                {
                    mangas.Add(new Manga
                    {
                        Titulo = $"Manga {i}",
                        Autor = $"Autor {i}",
                        Estado = _estados[_random.Next(_estados.Length)],
                        NumeroCapitulos = _random.Next(1, 1000),
                        Descripcion = $"Descripción del manga {i}",
                        Genero = _generos[_random.Next(_generos.Length)],
                        FechaPublicacion = DateTime.Now.AddDays(-_random.Next(1, 3650)),
                        ImagenUrl = $"https://example.com/manga{i}.jpg",
                        Calificacion = Math.Round(_random.NextDouble() * 5, 1)
                    });

                    if (i % 100 == 0)
                    {
                        _logger.LogInformation($"Procesados {i} mangas...");
                    }
                }

                _logger.LogInformation("Agregando mangas a la base de datos...");
                await _context.Mangas.AddRangeAsync(mangas);
                
                _logger.LogInformation("Guardando cambios...");
                var result = await _context.SaveChangesAsync();
                
                _logger.LogInformation($"Se insertaron {result} mangas exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sembrar datos de mangas");
                throw;
            }
        }
    }
} 