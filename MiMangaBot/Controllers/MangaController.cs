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
} 