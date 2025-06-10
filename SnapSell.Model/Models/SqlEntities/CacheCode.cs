namespace SnapSell.Domain.Models.SqlEntities;

public class CacheCode
{
    public int Version { get; set; }
    public required string LastUpdated { get; set; }
    public required string CacheKey { get; set; }
    public string? UserId { get; set; }
}