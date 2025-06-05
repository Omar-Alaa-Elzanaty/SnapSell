using Microsoft.AspNetCore.Identity;

namespace SnapSell.Domain.Models;

public class Account: IdentityUser
{
    public required string FullName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastUpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}