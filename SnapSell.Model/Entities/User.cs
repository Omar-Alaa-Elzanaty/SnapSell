using Microsoft.AspNetCore.Identity;

namespace SnapSell.Domain.Entities
{
    public class User : IdentityUser
    {
        public string UserId { get; set; }
        public required string Name { get; set; }
        public DateTime JoinDate { get; set; }
        public decimal WalletBalance { get; set; }

        // n.p
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<WalletTransaction> WalletTransactions { get; set; }
    }

}