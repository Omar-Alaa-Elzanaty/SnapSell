namespace SnapSell.Domain.Models;

public sealed class Size:BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}