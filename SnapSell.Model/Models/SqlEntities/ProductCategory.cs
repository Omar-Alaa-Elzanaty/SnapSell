namespace SnapSell.Domain.Models.SqlEntities;

public class ProductCategory
{
    public int ProductId { get; set; }
    public virtual Product Product { get; set; } 
    
    public Guid CategoryId { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public virtual Category? Category { get; set; }
}