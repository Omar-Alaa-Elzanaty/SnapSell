namespace SnapSell.Domain.Entities
{
    public sealed class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public required string PaymentMethod { get; set; } // From Enum ==> "CreditCard", "PayPal",
        public required string Status { get; set; } // From Enum ==> "Pending", "Completed", "Failed"
        public required string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public required string FailureReason { get; set; }

        // N.P
        public required Order Order { get; set; }
    }
}
