using SnapSell.Domain.Enums;

namespace SnapSell.Domain.Entities
{
    public class WalletTransaction
    {
        public Guid Id { get; set; }
        public required string UserId { get; set; }
        public User User { get; set; }
        public WalletTransactionType TransactionType { get; set; }
        public WalletTransactionStatus Status { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public Guid? PaymentId { get; set; }
        public Payment Payment { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; }
        public required string Reference { get; set; } // ==> External reference (bank transaction ID)
    }
}
