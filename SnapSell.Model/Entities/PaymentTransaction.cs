namespace SnapSell.Domain.Entities
{
    public class PaymentTransaction
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; }
        public required string TransactionType { get; set; }  // ==> "refund" ,, .and so on
        public decimal Amount { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime CreatedAt { get; set; } 

    }
}