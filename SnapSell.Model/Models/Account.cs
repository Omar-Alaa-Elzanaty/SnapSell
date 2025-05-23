using Microsoft.AspNetCore.Identity;

namespace SnapSell.Domain.Models;

public sealed class Account: IdentityUser
{
    public required string FullName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastUpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public List<Order> Orders { get; set; } = [];
    public List<OrderAddress> Addresses { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];
    public List<Product> Products { get; set; }
}