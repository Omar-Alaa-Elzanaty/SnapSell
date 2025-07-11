namespace SnapSell.Domain.Models.SqlEntities;

public class ProductBrand
{
    public int ProductId { get; set; }
    public virtual Product Product { get; set; } 
    
    public Guid BrandId { get; set; }
    public virtual Brand Brand { get; set; }
}