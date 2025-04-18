namespace SnapSell.Domain.Entities
{
    public class PaymentMethod
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public bool IsActive { get; set; }
    }
}
