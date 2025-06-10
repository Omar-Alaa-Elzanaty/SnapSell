using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.MongoDbEntities;

namespace SnapSell.Domain.Models.SqlEntities
{
    public class Store
    {
        public string Id { get; set; }
        public virtual Account Account { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinimumDeliverPeriod { get; set; }
        public int MaximumDeliverPeriod { get; set; }
        public StoreStatusTypes Status { get; set; } = StoreStatusTypes.Pending;
        public virtual DeliverPeriodTypes DeliverPeriodTypes { get; set; }
        public string? LogoUrl { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
