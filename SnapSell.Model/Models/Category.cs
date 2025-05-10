namespace SnapSell.Domain.Models;

public sealed class Category:BaseEntity
{
    public required string Name { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
}