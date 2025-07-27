using Microsoft.AspNetCore.Identity;
using SnapSell.Domain.Enums;

namespace SnapSell.Domain.Models.SqlEntities.Identitiy;

public class Account: IdentityUser
{
    public string FullName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastUpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public string Country { get; set; }
    public virtual Store Store { get; set; }
    
    public string? About { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    public virtual List<OrderAddress> Addresses { get; set; } = [];
    public virtual List<Review> Reviews { get; set; } = [];
    
    public virtual List<ClientCategoryFavorite> FavoriteCategories { get; set; } = [];
    public virtual List<ClientBrandFavorite> FavoriteBrands { get; set; } = [];
    
}