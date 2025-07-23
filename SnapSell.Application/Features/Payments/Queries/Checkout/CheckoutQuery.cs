using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Payments.Queries.Checkout
{
    public class CheckoutQuery:IRequest<Result<List<CheckoutQueryDto>>>
    {

    }

    public class CheckoutQueryDto
    {
        public string PhoneNumber { get; set; }
        public required string City { get; set; }
        public required string District { get; set; }
        public bool IsDefault { get; set; }
    }
}
