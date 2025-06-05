namespace SnapSell.Domain.Models;

public class ShoppingBag:BaseEntity
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}