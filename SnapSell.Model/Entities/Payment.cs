using SnapSell.Domain.Enums;

namespace SnapSell.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public bool IsWalletPayment { get; set; }
        public int? WalletTransactionId { get; set; }
        public WalletTransaction WalletTransaction { get; set; }
        public required string TransactionId { get; set; }
        public required string PaymentKey { get; set; }
        public required string MerchantOrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
