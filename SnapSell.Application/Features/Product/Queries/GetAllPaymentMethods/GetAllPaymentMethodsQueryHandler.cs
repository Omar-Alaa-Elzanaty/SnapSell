using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models;
using System.Net;
using Mapster;

namespace SnapSell.Application.Features.product.Queries.GetAllPaymentMethods;

internal sealed class GetAllPaymentMethodsQueryHandler(
    ISQLBaseRepo<PaymentMethod> paymentMethodRepository,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<GetAllPaymentMethodsQuery,
        Result<List<GetAllPaymentMethodsResponse>>>
{
    public async Task<Result<List<GetAllPaymentMethodsResponse>>> Handle(GetAllPaymentMethodsQuery request,
    CancellationToken cancellationToken)
    {
        var paymentMethods = await paymentMethodRepository.GetAllAsync();

        return Result<List<GetAllPaymentMethodsResponse>>.Success(
            data: paymentMethods.Adapt<List<GetAllPaymentMethodsResponse>>(),
            message: "Payment Methods returned successfully.",
            HttpStatusCode.OK);
    }
}