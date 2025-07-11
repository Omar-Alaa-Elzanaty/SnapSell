namespace SnapSell.Domain.Models.SqlEntities;

public class Variant
{
    public Guid Id { get; set; }
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    public Guid SizeId { get; set; }
    public virtual Size Size { get; set; }
    public string Color { get; set; } = string.Empty;

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public decimal? SalePrice { get; set; }

    public decimal? CostPrice { get; set; }

    public string? Sku { get; set; }

    public bool IsDefault { get; set; }
}