using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.product.Queries.GetAllPaymentMethods;

public sealed record GetAllPaymentMethodsQuery(string SellerId)
    : IRequest<Result<List<GetAllPaymentMethodsResponse>>>;

public sealed record GetAllPaymentMethodsResponse(Guid PaymentMethodId, string Name);