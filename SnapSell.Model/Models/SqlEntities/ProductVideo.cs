namespace SnapSell.Domain.Models.SqlEntities;

public class ProductVideo : Auditable
{
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    public Guid VideoId { get; set; }
    public virtual Video Video { get; set; }
}