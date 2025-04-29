
namespace SnapSell.Domain.Models;

public sealed class Review : BaseEntity
{
    public string UserId { get; set; }
    public User User { get; set; }
    public double Score { get; set; }
    public string? Content { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}