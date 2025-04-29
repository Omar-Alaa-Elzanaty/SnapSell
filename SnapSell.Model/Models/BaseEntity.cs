namespace SnapSell.Domain.Models;

public class BaseEntity : Auditable
{
    public Guid Id { get; set; }
}