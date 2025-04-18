namespace SnapSell.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductVariantId { get; set; }
        public ProductVariant? ProductVariant { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}