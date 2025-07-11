namespace SnapSell.Domain.Models.SqlEntities;

public class Category:BaseEntity
{
    public required string Name { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public virtual Category? ParentCategory { get; set; }
}