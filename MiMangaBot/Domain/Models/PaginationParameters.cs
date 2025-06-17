using System.ComponentModel.DataAnnotations;

namespace MiMangaBot.Domain.Models;

public class PaginationParameters
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;
    private int _pageNumber = 1;

    [Range(1, int.MaxValue, ErrorMessage = "El número de página debe ser mayor que 0")]
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    [Range(1, 50, ErrorMessage = "El tamaño de página debe estar entre 1 y 50")]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 1 : value;
    }
} 