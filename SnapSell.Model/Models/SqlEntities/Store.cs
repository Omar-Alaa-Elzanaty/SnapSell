using SnapSell.Domain.Enums;

namespace SnapSell.Domain.Models.SqlEntities
{
    public class Store
    {
        public Guid SellerId { get; set; }
        public Seller Seller { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinimumDeliverPeriod { get; set; }
        public int MaximumDeliverPeriod { get; set; }
        public StoreStatusTypes Status { get; set; } = StoreStatusTypes.Pending;
        public virtual DeliverPeriodTypes DeliverPeriodTypes { get; set; }
        public string? LogoUrl { get; set; }
    }
}
