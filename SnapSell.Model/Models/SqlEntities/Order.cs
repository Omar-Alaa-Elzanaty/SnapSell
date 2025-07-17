using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Domain.Models.SqlEntities;

public class Order : Auditable
{
    public int Id { get; set; }
    public string ClientId { get; set; }  // customerId
    public virtual Client Client { get; set; }
    public virtual List<OrderItem> Items { get; set; } = [];
    public virtual OrderAddress ShippingAddress { get; set; }
    public required Guid ShippingAddressId { get; set; }
    public virtual OrderAddress? BillingAddress { get; set; }
    public Guid? BillingAddressId { get; set; }
    public string PaymentMethod { get; set; }
    public string Email { get; set; }
    public decimal OrderTotal { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
}

public class OrderItem : BaseEntity
{
    public int ProductId { get; set; }
    public Guid? VariantId { get; set; }

    public required int OrderId { get; set; }
    public virtual Order Order { get; set; }
    public int Quantity { get; set; }
    public decimal ProductVariantUnitPrice { get; set; }
    public decimal TotalPrice => Quantity * ProductVariantUnitPrice;
}

public class OrderAddress : BaseEntity
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
    public string ClientId { get; set; }
    public virtual Client Client { get; set; }
    public bool IsDefault { get; set; }
}