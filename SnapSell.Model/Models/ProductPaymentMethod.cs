namespace SnapSell.Domain.Models;

public class ProductPaymentMethod
{
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }

    public Guid PaymentMethodId { get; set; }
    public virtual PaymentMethod PaymentMethod { get; set; }
}