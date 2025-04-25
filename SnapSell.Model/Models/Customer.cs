namespace SnapSell.Domain.Models
{
    public class Customer:ApplicationUser
    {
        public List<Order> Orders { get; set; }

    }
}
