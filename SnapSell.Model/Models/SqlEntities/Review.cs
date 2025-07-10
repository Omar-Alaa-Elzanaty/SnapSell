using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Domain.Models.SqlEntities;

public class Review : BaseEntity
{
    public string UserId { get; set; }
    public virtual Account User { get; set; }
    public double Score { get; set; }
    public string? Content { get; set; }
    public int ProductId { get; set; }
}