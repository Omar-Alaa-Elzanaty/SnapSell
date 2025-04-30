namespace SnapSell.Domain.Models;

public sealed class Product : BaseEntity
{
    public required string EnglishName { get; set; }
    public required string ArabicName { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public List<Category> Categories { get; set; }
    public List<Size> Sizes { get; set; }
    public List<Color> Colors { get; set; }
    public List<Variant> Variants { get; set; }
    public List<Review> Reviews { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsHidden { get; set; }
    public Guid BrandId { get; set; }
    public Brand Brand { get; set; }
    public int MinDeleveryDays { get; set; }
    public int MaxDeleveryDays { get; set; }
    public string? MainImageUrl { get; set; }
    public List<string> AdditionalImageUrls { get; set; } = [];
}