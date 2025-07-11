namespace SnapSell.Domain.Models.SqlEntities;

public class BaseEntity : Auditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
}