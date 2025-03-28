using Microsoft.AspNetCore.Identity;

namespace SnapSell.Domain.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public required string FirstName { get; set; }
        public required string MiddleName { get; set; }
        public required string LastName { get; set; }
        public string FullName => $"{FirstName} {MiddleName} {LastName}";
    }
}
