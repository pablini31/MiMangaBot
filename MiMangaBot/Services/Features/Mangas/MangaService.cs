using Microsoft.EntityFrameworkCore;
using MiMangaBot.Domain.Data;
using MiMangaBot.Domain.Entities;

namespace MiMangaBot.Services.Features.Mangas;

public class MangaService
{
    private readonly ILogger<MangaService> _logger;
    private readonly ApplicationDbContext _context;

    public MangaService(ILogger<MangaService> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IEnumerable<Manga>> GetAllMangasAsync()
    {
        _logger.LogInformation("Obteniendo todos los mangas");
        return await _context.Manga.ToListAsync();
    }

    public async Task<Manga?> GetMangaByIdAsync(int id)
    {
        _logger.LogInformation($"Buscando manga con ID: {id}");
        return await _context.Manga.FindAsync(id);
    }

    public async Task<Manga> CreateMangaAsync(Manga manga)
    {
        _logger.LogInformation($"Agregando nuevo manga: {manga.Titulo}");
        _context.Manga.Add(manga);
        await _context.SaveChangesAsync();
        return manga;
    }

    public async Task<bool> UpdateMangaAsync(Manga updatedManga)
    {
        _logger.LogInformation($"Actualizando manga con ID: {updatedManga.Id}");
        var existingManga = await _context.Manga.FindAsync(updatedManga.Id);
        if (existingManga == null)
            return false;

        existingManga.Titulo = updatedManga.Titulo;
        existingManga.Autor = updatedManga.Autor;
        existingManga.Genero = updatedManga.Genero;
        existingManga.Capitulos = updatedManga.Capitulos;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteMangaAsync(int id)
    {
        _logger.LogInformation($"Eliminando manga con ID: {id}");
        var manga = await _context.Manga.FindAsync(id);
        if (manga == null)
            return false;

        _context.Manga.Remove(manga);
        await _context.SaveChangesAsync();
        return true;
    }
} 