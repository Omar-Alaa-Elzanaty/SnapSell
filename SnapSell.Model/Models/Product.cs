using SnapSell.Domain.Enums;

namespace SnapSell.Domain.Models;

public sealed class Product : BaseEntity
{
    public required string EnglishName { get; set; }
    public required string ArabicName { get; set; }
    public string? EnglishDescription { get; set; }
    public string? ArabicDescription { get; set; }
    public List<Category> Categories { get; set; }
    public List<Size> Sizes { get; set; }
    public List<Color> Colors { get; set; }
    public ICollection<Variant> Variants { get; set; } = new HashSet<Variant>();
    public ICollection<ProductPaymentMethod> ProductPaymentMethods { get; set; } = new HashSet<ProductPaymentMethod>();
    public List<Review> Reviews { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsHidden { get; set; }
    public Guid BrandId { get; set; }
    public Brand Brand { get; set; }
    public ProductStatus? ProductStatus { get; set; }
    public int MinDeleveryDays { get; set; }
    public int MaxDeleveryDays { get; set; }
    public string? MainImageUrl { get; set; }
    public string? MainVideoUrl { get; set; }
    public List<string> AdditionalImageUrls { get; set; } = [];
    public ShippingType? ShippingType { get; set; }
}