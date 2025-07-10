namespace SnapSell.Domain.Models.SqlEntities;

public class ProductImage:BaseEntity
{
    public virtual Product Product{ get; set; }
    public int ProductId{ get; set; }
    public string ImageUrl { get; set; } = null!;
    public bool IsMainImage { get; set; }
}