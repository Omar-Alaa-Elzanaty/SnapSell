using System.ComponentModel.DataAnnotations;

namespace SnapSell.Domain.Models;

public sealed class Order : BaseEntity
{
    public string UserId { get; set; }
    public User User { get; set; }
    public List<OrderItem> Items { get; set; } = [];
    public OrderAddress ShippingAddress { get; set; }
    public required Guid ShippingAddressId { get; set; }
    public OrderAddress? BillingAddress { get; set; }
    public Guid? BillingAddressId { get; set; }
    public string PaymentMethod { get; set; }
    public string Email { get; set; }
    public decimal OrderTotal { get; set; }
    public string OrderStatus { get; set; } = "Pending";
}

public sealed class OrderItem : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public required Guid OrderId { get; set; }
    public Order Order { get; set; }
    public Guid? VariantId { get; set; }
    public Variant Variant { get; set; }
    public int Quantity { get; set; }
    public decimal ProductVariantUnitPrice { get; set; }
    public decimal TotalPrice => Quantity * ProductVariantUnitPrice;
}
public sealed class OrderAddress:BaseEntity
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