using System.ComponentModel.DataAnnotations;

namespace SnapSell.Domain.Dtos;

public record PaginatedRequest
{
    private int _pageNumber = 1;
    private int _pageSize = 10;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value > 0 ? value : 1;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value is > 0 and <= 50 ? value : 10;
    }

    public string? SortBy { get; set; }
    public string SortOrder { get; set; } = "asc";
    public string? KeyWord { get; set; }
}