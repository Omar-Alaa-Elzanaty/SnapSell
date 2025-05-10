using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models;
using System.Net;
using System.Security.Claims;
using Mapster;
using SnapSell.Application.DTOs.payment;

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
        var currentUser = httpContextAccessor.HttpContext?.User;
        if (currentUser is null)
        {
            return Result<List<GetAllPaymentMethodsResponse>>.Failure(
                message: "Current user is null",
                HttpStatusCode.Unauthorized);
        }

        var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<List<GetAllPaymentMethodsResponse>>.Failure(
                message: "User ID not found in claims",
                HttpStatusCode.Unauthorized);
        }

        var paymentMethods = await paymentMethodRepository.GetAllAsync();
        if (!paymentMethods.Any())
        {
            return Result<List<GetAllPaymentMethodsResponse>>.Failure(
                message: "Currently There is no Payment Methods.",
                HttpStatusCode.NotFound);
        }

        return Result<List<GetAllPaymentMethodsResponse>>.Success(
            data: paymentMethods.Adapt<List<GetAllPaymentMethodsResponse>>(),
            message: "Payment Methods returned successfully.",
            HttpStatusCode.OK);
    }
}