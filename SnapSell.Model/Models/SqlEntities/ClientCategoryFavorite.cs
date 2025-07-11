using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Domain.Models.SqlEntities;

public class ClientCategoryFavorite
{
    public string ClientId { get; set; }
    public virtual Client Client { get; set; }
    
    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; }
    
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
}