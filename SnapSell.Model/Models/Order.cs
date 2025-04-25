namespace SnapSell.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<OrderItem> Items { get; set; }
        public OrderAddress ShippingAddress { get; set; }
        public OrderAddress? BillingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string Email { get; set; }
        public decimal OrderTotal { get; set; }

        public string OrderStatus { get; set; } = "Pending";
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int VariantId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

    }
    public class OrderAddress
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public required string StreetName { get; set; }
        public required string BuildingDetails { get; set; }
        public required string Country { get; set; }
        public required string Governorate { get; set; }
        public required string City { get; set; }
        public required string District { get; set; }
        public string? Landmark { get; set; }
        public bool IsDefault { get; set; }
    }
}
