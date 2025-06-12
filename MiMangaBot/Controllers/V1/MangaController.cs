using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiMangaBot.Domain.Data;
using MiMangaBot.Domain.Entities;
using MiMangaBot.Services.Features.Mangas;
using MiMangaBot.Scripts;

namespace MiMangaBot.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class MangaController : ControllerBase
{
    private readonly MangaService _mangaService;
    private readonly ILogger<MangaController> _logger;
    private readonly MangaDataSeeder _seeder;
    private readonly ApplicationDbContext _context;

    public MangaController(MangaService mangaService, ILogger<MangaController> logger, MangaDataSeeder seeder, ApplicationDbContext context)
    {
        _mangaService = mangaService;
        _logger = logger;
        _seeder = seeder;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Manga>>> GetAllMangas()
    {
        try
        {
            var mangas = await _mangaService.GetAllMangasAsync();
            return Ok(mangas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los mangas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Manga>> GetMangaById(int id)
    {
        try
        {
            var manga = await _mangaService.GetMangaByIdAsync(id);
            if (manga == null)
                return NotFound($"Manga con ID {id} no encontrado");

            return Ok(manga);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener manga con ID {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Manga>> CreateManga(Manga manga)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdManga = await _mangaService.AddMangaAsync(manga);
            return CreatedAtAction(nameof(GetMangaById), new { id = createdManga.Id }, createdManga);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear nuevo manga");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Manga>> UpdateManga(int id, Manga manga)
    {
        try
        {
            if (id != manga.Id)
                return BadRequest("ID no coincide");

            var updatedManga = await _mangaService.UpdateMangaAsync(id, manga);
            if (updatedManga == null)
                return NotFound($"Manga con ID {id} no encontrado");

            return Ok(updatedManga);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar manga con ID {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteManga(int id)
    {
        try
        {
            var result = await _mangaService.DeleteMangaAsync(id);
            if (!result)
                return NotFound($"Manga con ID {id} no encontrado");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al eliminar manga con ID {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Manga>>> SearchMangas([FromQuery] string query)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("El término de búsqueda no puede estar vacío");

            var searchResults = await _mangaService.SearchMangasAsync(query);
            return Ok(searchResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al buscar mangas con término: {query}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedMangas([FromQuery] int count = 3500)
    {
        try
        {
            await _seeder.SeedMangasAsync(count);
            return Ok(new { 
                message = $"Se han insertado {count} mangas exitosamente",
                timestamp = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al sembrar datos de mangas");
            return StatusCode(500, new { 
                error = "Error al sembrar datos de mangas",
                details = ex.Message,
                timestamp = DateTime.Now
            });
        }
    }

    [HttpGet("test-connection")]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            var count = await _context.Mangas.CountAsync();
            return Ok(new { 
                message = "Conexión exitosa a la base de datos",
                mangasCount = count,
                timestamp = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al conectar con la base de datos");
            return StatusCode(500, new { 
                error = "Error al conectar con la base de datos",
                details = ex.Message,
                timestamp = DateTime.Now
            });
        }
    }

    [HttpGet("duplicates")]
    public async Task<IActionResult> GetDuplicateMangas()
    {
        try
        {
            var duplicates = await _context.Mangas
                .GroupBy(m => m.Titulo)
                .Where(g => g.Count() > 1)
                .Select(g => new
                {
                    Titulo = g.Key,
                    Cantidad = g.Count(),
                    Mangas = g.Select(m => new
                    {
                        m.Id,
                        m.Titulo,
                        m.Autor,
                        m.Estado,
                        m.NumeroCapitulos,
                        m.Genero,
                        m.FechaPublicacion
                    }).ToList()
                })
                .ToListAsync();

            return Ok(new
            {
                totalDuplicados = duplicates.Count,
                mangasDuplicados = duplicates
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar mangas duplicados");
            return StatusCode(500, new
            {
                error = "Error al buscar mangas duplicados",
                details = ex.Message,
                timestamp = DateTime.Now
            });
        }
    }
} 