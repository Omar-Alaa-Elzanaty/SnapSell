namespace SnapSell.Domain.Models.SqlEntities;

public class ShoppingBag:BaseEntity
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}