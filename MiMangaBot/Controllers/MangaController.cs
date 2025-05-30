using Microsoft.AspNetCore.Mvc;

namespace MiMangaBot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MangaController : ControllerBase
{
    private readonly ILogger<MangaController> _logger;

    public MangaController(ILogger<MangaController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "API de Manga funcionando correctamente!" });
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        try
        {
            var response = new
            {
                status = "success",
                message = "El endpoint de prueba está funcionando",
                timestamp = DateTime.Now,
                version = "1.0"
            };
            
            _logger.LogInformation("Test endpoint called successfully");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en el endpoint de prueba");
            return StatusCode(500, new { error = "Error interno del servidor" });
        }
    }
} 