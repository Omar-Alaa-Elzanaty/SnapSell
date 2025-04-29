namespace SnapSell.Domain.Interfaces;

public interface IAuditable
{
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastUpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}