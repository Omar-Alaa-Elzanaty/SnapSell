namespace SnapSell.Domain.Models;

public class Review : BaseEntity
{
    public string UserId { get; set; }
    public virtual Account User { get; set; }
    public double Score { get; set; }
    public string? Content { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
}