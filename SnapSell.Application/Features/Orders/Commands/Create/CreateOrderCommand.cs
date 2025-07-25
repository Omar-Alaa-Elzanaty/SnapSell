using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.Orders.Commands.Create
{
    public class CreateOrderCommand : IRequest<Result<string>>
    {
        public List<OrderVarientDto>Varients { get; set; }
        public string? Notes { get; set; }
        public Guid ShippingAddressId { get; set; }
        public string? VoucherCode { get; set; }
        public Guid? BillingAddressId { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
        public Currencies Currency { get; set; }
    }

    public class OrderVarientDto
    {
        public Guid VariantId { get; set; }
        public int Quantity { get; set; }
    }
}
