using System.ComponentModel.DataAnnotations;

namespace MiMangaBot.Domain.Entities;

public class Manga
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Titulo { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string? Autor { get; set; }
    
    [StringLength(50)]
    public string? Estado { get; set; } // En emisión, Finalizado, etc.
    
    public int NumeroCapitulos { get; set; }
    
    [StringLength(500)]
    public string? Descripcion { get; set; }
    
    [StringLength(100)]
    public string? Genero { get; set; }
    
    public DateTime FechaPublicacion { get; set; }
    
    [StringLength(200)]
    public string? ImagenUrl { get; set; }
    
    public double Calificacion { get; set; }
} 