using SnapSell.Domain.Enums;

namespace SnapSell.Domain.Models.SqlEntities
{
    public class Payment : Auditable
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int PaymobTransactionId { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    }
}
