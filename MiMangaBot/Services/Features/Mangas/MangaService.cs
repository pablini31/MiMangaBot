using MiMangaBot.Domain.Entities;

namespace MiMangaBot.Services.Features.Mangas;

public class MangaService
{
    private readonly ILogger<MangaService> _logger;
    private readonly List<Manga> _mangas;

    public MangaService(ILogger<MangaService> logger)
    {
        _logger = logger;
        _mangas = new List<Manga>();
    }

    public async Task<IEnumerable<Manga>> GetAllMangasAsync()
    {
        _logger.LogInformation("Obteniendo todos los mangas");
        return await Task.FromResult(_mangas);
    }

    public async Task<Manga?> GetMangaByIdAsync(int id)
    {
        _logger.LogInformation($"Buscando manga con ID: {id}");
        return await Task.FromResult(_mangas.FirstOrDefault(m => m.Id == id));
    }

    public async Task<Manga> AddMangaAsync(Manga manga)
    {
        _logger.LogInformation($"Agregando nuevo manga: {manga.Titulo}");
        manga.Id = _mangas.Count + 1;
        manga.FechaPublicacion = DateTime.UtcNow;
        _mangas.Add(manga);
        return await Task.FromResult(manga);
    }

    public async Task<Manga?> UpdateMangaAsync(int id, Manga updatedManga)
    {
        _logger.LogInformation($"Actualizando manga con ID: {id}");
        var existingManga = _mangas.FirstOrDefault(m => m.Id == id);
        if (existingManga == null)
            return null;

        existingManga.Titulo = updatedManga.Titulo;
        existingManga.Autor = updatedManga.Autor;
        existingManga.Descripcion = updatedManga.Descripcion;
        existingManga.ImagenUrl = updatedManga.ImagenUrl;
        existingManga.NumeroCapitulos = updatedManga.NumeroCapitulos;
        existingManga.Estado = updatedManga.Estado;
        existingManga.Genero = updatedManga.Genero;
        existingManga.Calificacion = updatedManga.Calificacion;

        return await Task.FromResult(existingManga);
    }

    public async Task<bool> DeleteMangaAsync(int id)
    {
        _logger.LogInformation($"Eliminando manga con ID: {id}");
        var manga = _mangas.FirstOrDefault(m => m.Id == id);
        if (manga == null)
            return false;

        _mangas.Remove(manga);
        return await Task.FromResult(true);
    }

    public async Task<IEnumerable<Manga>> SearchMangasAsync(string searchTerm)
    {
        _logger.LogInformation($"Buscando mangas con término: {searchTerm}");
        return await Task.FromResult(_mangas.Where(m => 
            (m.Titulo?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (m.Autor?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (m.Descripcion?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false)));
    }
} 