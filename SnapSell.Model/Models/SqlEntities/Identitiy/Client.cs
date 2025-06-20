namespace SnapSell.Domain.Models.SqlEntities.Identitiy;

public class Client:Account
{
    
    public string About { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    public virtual List<OrderAddress> Addresses { get; set; } = [];
    public virtual List<Review> Reviews { get; set; } = [];
}