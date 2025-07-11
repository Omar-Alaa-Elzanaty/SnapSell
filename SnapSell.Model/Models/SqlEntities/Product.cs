using SnapSell.Domain.Enums;

namespace SnapSell.Domain.Models.SqlEntities;

public class Product : Auditable
{
    public int Id { get; set; }

    public string EnglishName { get; set; }
    public string ArabicName { get; set; }

    public Guid StoreId { get; set; }
    public string? EnglishDescription { get; set; }
    public string? ArabicDescription { get; set; }
    public List<Guid> CategoryIds { get; set; } = new();
    public virtual List<Variant> Variants { get; set; } = new();

    public List<int> PaymentMethods { get; set; } = new();

    public bool IsFeatured { get; set; }

    public bool IsHidden { get; set; }
    public bool HasVariants { get; set; }
    public Guid BrandId { get; set; }
    public virtual Brand Brand { get; set; }

    public ProductStatus ProductStatus { get; set; }
    public ProductTypes ProductType { get; set; }
    public int MinDeliveryDays { get; set; }
    public int MaxDeliveryDays { get; set; }
    public virtual List<ProductImage> Images { get; set; } = new();
    public virtual List<ProductVideo> Videos { get; set; } = new();
    public ShippingType ShippingType { get; set; }
    public decimal? Price { get; set; }
    public decimal? SalePrice { get; set; }
    public decimal? CostPrice { get; set; }
    public int? Quantity { get; set; }
    public string? Sku { get; set; }
    public bool IsDraft { get; set; }
}