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

        // Payment source (could be wallet or external)
        public bool IsWalletPayment { get; set; }
        public int? WalletTransactionId { get; set; }
        public WalletTransaction WalletTransaction { get; set; }

        // Payment details
        public required string TransactionId { get; set; }       // Paymob transaction ID
        public required string PaymentKey { get; set; }          // Paymob payment key
        public required string MerchantOrderId { get; set; }     // Paymob order ID
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Card details (if applicable)
        public string MaskedPan { get; set; }          // "XXXX-XXXX-XXXX-1234"
        public string CardType { get; set; }           // "Visa", "MasterCard", etc.
        public string CardSubType { get; set; }         // "CREDIT", "DEBIT"

        // paymob data
        public bool IsPaymobPayment { get; set; }
        public int PaymobIntegrationId { get; set; }
        public required string PaymobPaymentToken { get; set; }

        // Cash on delivery specific
        public bool IsCashOnDelivery { get; set; }

        //error may happens during payment process handling
        public string? ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }

        // Navigation properties
        public ICollection<PaymentTransaction> Transactions { get; set; }
    }
}
