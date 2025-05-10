namespace SnapSell.Domain.Models;

public class ProductPaymentMethod
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    public Guid PaymentMethodId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}