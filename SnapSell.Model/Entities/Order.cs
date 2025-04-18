using SnapSell.Domain.Enums;

namespace SnapSell.Domain.Entities
{
    public abstract class Order
    {
        public Guid Id { get; set; }
        public required string OrderNumber { get; set; }
        public required string UserId { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

    public class CustomerOrder : Order
    {
        public int ShippingAddressId { get; set; }
        public required Address ShippingAddress { get; set; }
        public int? BillingAddressId { get; set; }
        public Address? BillingAddress { get; set; }
        public required string PaymentMethod { get; set; }
        public required string Email { get; set; }
        public string? VoucherCode { get; set; }
        public string? OrderNotes { get; set; }
        public decimal ShippingAmount { get; set; }
        public Payment? Payment { get; set; }

    }

    public class ProvisionOrder : Order
    {
        // Seller restocking fields
       
    }
}
