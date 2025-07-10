using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Domain.Models.SqlEntities;

public class ClientBrandFavorite
{
    public string ClientId { get; set; }
    public virtual Client Client { get; set; }
    
    public Guid BrandId { get; set; }
    public virtual Brand Brand { get; set; }
    
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
}