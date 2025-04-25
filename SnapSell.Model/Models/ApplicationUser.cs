using Microsoft.AspNetCore.Identity;
using SnapSell.Domain.Interfaces;

namespace SnapSell.Domain.Models
{
    public class ApplicationUser: IdentityUser,IAuditable
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
