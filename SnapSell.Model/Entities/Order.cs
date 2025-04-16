using SnapSell.Domain.Enums;

namespace SnapSell.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public required string UserId { get; set; }
        public User User { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
