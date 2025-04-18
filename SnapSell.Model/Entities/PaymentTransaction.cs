namespace SnapSell.Domain.Entities
{
    public class PaymentTransaction
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; }

        // Transaction details
        public required string TransactionType { get; set; }      // "auth", "capture", "void", "refund"
        public decimal Amount { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime CreatedAt { get; set; } 

        // Gateway_response
        public string GatewayTransactionId { get; set; }
        public string GatewayResponse { get; set; }     //==> response from gateway
        public string GatewayStatusCode { get; set; }
        public string GatewayMessage { get; set; }
    }
}
