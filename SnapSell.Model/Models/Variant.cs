namespace SnapSell.Domain.Models;

public class Variant : BaseEntity
{
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    public string? Size { get; set; }
    public string? Color { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal RegularPrice { get; set; }
    public decimal? SalePrice { get; set; }
    public string? SKU { get; set; }
}