namespace SnapSell.Domain.Models.SqlEntities;

public class Size:BaseEntity
{
    public string? Name { get; set; }
    public Guid? ParentSizeId { get; set; }
    public virtual Size? ParentSize { get; set; }
}