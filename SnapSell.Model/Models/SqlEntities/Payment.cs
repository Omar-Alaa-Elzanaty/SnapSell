using SnapSell.Domain.Enums;

namespace SnapSell.Domain.Models.SqlEntities
{
    public class Payment : Auditable
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public long? PaymobTransactionId { get; set; }
        public long IntegrationId { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    }
}
