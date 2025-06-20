namespace SnapSell.Domain.Models.MongoDbEntities;

public class ProductImage
{
    public string ImageUrl { get; set; } = null!;
    public bool IsMainImage { get; set; }
}