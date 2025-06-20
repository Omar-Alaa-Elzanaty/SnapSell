using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Domain.Models.SqlEntities;

public class Store : BaseEntity
{
    public string? SellerId { get; set; }
    public virtual Seller Seller { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int MinimumDeliverPeriod { get; set; }
    public int MaximumDeliverPeriod { get; set; }
    public StoreStatusTypes Status { get; set; } = StoreStatusTypes.Pending;
    public DeliverPeriodTypes DeliverPeriodTypes { get; set; }
    public string? LogoUrl { get; set; }

  }