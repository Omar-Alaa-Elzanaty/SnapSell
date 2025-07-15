namespace SnapSell.Domain.Models.SqlEntities;

public class Brand:BaseEntity
{
    public string Name { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
}