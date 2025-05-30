namespace MiMangaBot.Domain.Entities;

public class Manga
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public int TotalChapters { get; set; }
    public string Status { get; set; } = string.Empty; // Ongoing, Completed, Hiatus
    public List<string> Genres { get; set; } = new();
    public DateTime ReleaseDate { get; set; }
    public DateTime LastUpdated { get; set; }
    public double Rating { get; set; }
    public int Views { get; set; }
    public bool IsPopular { get; set; }
    public bool IsCompleted { get; set; }
} 