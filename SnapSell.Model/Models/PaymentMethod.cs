namespace SnapSell.Domain.Models;

public class PaymentMethod : BaseEntity
{
    public required string Name { get; set; } = string.Empty;
    public virtual ICollection<ProductPaymentMethod> ProductPaymentMethods { get; set; } = new HashSet<ProductPaymentMethod>();

}