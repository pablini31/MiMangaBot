using Microsoft.AspNetCore.Mvc;
using MiMangaBot.Domain.Entities;
using MiMangaBot.Services.Features.Mangas;

namespace MiMangaBot.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class MangaController : ControllerBase
{
    private readonly MangaService _mangaService;
    private readonly ILogger<MangaController> _logger;

    public MangaController(MangaService mangaService, ILogger<MangaController> logger)
    {
        _mangaService = mangaService;
        _logger = logger;
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

            var createdManga = await _mangaService.CreateMangaAsync(manga);
            return CreatedAtAction(nameof(GetMangaById), new { id = createdManga.Id }, createdManga);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear nuevo manga");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateManga(int id, Manga manga)
    {
        try
        {
            if (id != manga.Id)
                return BadRequest("ID no coincide");

            var updated = await _mangaService.UpdateMangaAsync(manga);
            if (!updated)
                return NotFound($"Manga con ID {id} no encontrado");

            return NoContent();
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
} 