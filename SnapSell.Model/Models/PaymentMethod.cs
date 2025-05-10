namespace SnapSell.Domain.Models;

public sealed class PaymentMethod : BaseEntity
{
    public required string Name { get; set; } = string.Empty;
    public ICollection<ProductPaymentMethod> ProductPaymentMethods { get; set; } = new HashSet<ProductPaymentMethod>();

}