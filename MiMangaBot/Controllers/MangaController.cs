using Microsoft.AspNetCore.Mvc;
using MiMangaBot.Domain.Entities;
using MiMangaBot.Services.Features.Mangas;

namespace MiMangaBot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MangaController : ControllerBase
{
    private readonly ILogger<MangaController> _logger;
    private readonly MangaService _mangaService;

    public MangaController(ILogger<MangaController> logger, MangaService mangaService)
    {
        _logger = logger;
        _mangaService = mangaService;
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        _logger.LogInformation("Test endpoint called");
        return Ok(new { 
            status = "success",
            message = "API funcionando correctamente",
            timestamp = DateTime.Now
        });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Manga>>> GetAll()
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
    public async Task<ActionResult<Manga>> GetById(int id)
    {
        try
        {
            var manga = await _mangaService.GetMangaByIdAsync(id);
            if (manga == null)
                return NotFound($"No se encontró el manga con ID {id}");
            
            return Ok(manga);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener el manga con ID {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Manga>> Create(Manga manga)
    {
        try
        {
            var nuevoManga = await _mangaService.CreateMangaAsync(manga);
            return CreatedAtAction(nameof(GetById), new { id = nuevoManga.Id }, nuevoManga);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el manga");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Manga manga)
    {
        try
        {
            if (id != manga.Id)
                return BadRequest("El ID del manga no coincide");

            var resultado = await _mangaService.UpdateMangaAsync(manga);
            if (!resultado)
                return NotFound($"No se encontró el manga con ID {id}");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar el manga con ID {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var resultado = await _mangaService.DeleteMangaAsync(id);
            if (!resultado)
                return NotFound($"No se encontró el manga con ID {id}");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al eliminar el manga con ID {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 