using System.ComponentModel.DataAnnotations;

namespace MiMangaBot.Domain.Entities;

public class Manga
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string? Autor { get; set; }
    
    [StringLength(50)]
    public string? Genero { get; set; }
    
    public int? Capitulos { get; set; }
} 