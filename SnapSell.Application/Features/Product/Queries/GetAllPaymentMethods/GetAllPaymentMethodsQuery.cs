using MediatR;
using SnapSell.Application.DTOs.payment;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.product.Queries.GetAllPaymentMethods;

public sealed record GetAllPaymentMethodsQuery(string SellerId)
    : IRequest<Result<List<GetAllPaymentMethodsResponse>>>;