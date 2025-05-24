using SnapSell.Domain.Enums;

namespace SnapSell.Domain.Models
{
    public class Store
    {
        public string Id { get; set; }
        public Account Account { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinimumDeliverPeriod { get; set; }
        public int MaximumDeliverPeriod { get; set; }
        public StoreStatusTypes Status { get; set; } = StoreStatusTypes.Pending;
        public DeliverPeriodTypes DeliverPeriodTypes { get; set; }
        public string? LogoUrl { get; set; }
    }
}
