using Microsoft.AspNetCore.Identity;

namespace SnapSell.Domain.Entities
{
    public class User : IdentityUser
    {
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public DateTime CreatedAt { get; set; }
        public required string FirstName { get; set; }
        public required string MiddleName { get; set; }
        public required string LastName { get; set; }
        public Guid OrderId { get; set; }
        public string FullName => $"{FirstName} {MiddleName} {LastName}";
    }
}