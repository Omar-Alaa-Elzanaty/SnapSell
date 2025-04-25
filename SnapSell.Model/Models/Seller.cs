namespace SnapSell.Domain.Models
{
    public class Seller : ApplicationUser
    {
        public List<Order> Orders { get; set; }
        public List<Product> Products { get; set; }


    }
}
