

namespace SnapSell.Domain.Models;

public sealed class Variant : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public Guid? SizeId { get; set; }
    public Size? Size { get; set; }
    public Guid? ColorId { get; set; }
    public Color? Color { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal RegularPrice { get; set; }
    public decimal? SalePrice { get; set; }
    public string? SKU { get; set; }
    public string? Barcode { get; set; }
}