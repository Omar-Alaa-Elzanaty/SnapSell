namespace SnapSell.Domain.Models.SqlEntities.Identitiy;

public class Seller:Account
{
    public virtual Store Store { get; set; }
}