using System.Net;
using System.Security.Claims;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Extensions;
using SnapSell.Application.Features.products.Queries.GetAllProductsForSpecificSeller;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Products.Queries.GetAllProductsForSpecificSeller;

internal sealed class GetAllProductsForSpecificSellerQueryHandler(
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<GetAllProductsForSpecificSellerQuery, PaginatedResult<GetAllProductsForSpecificSellerResponse>>
{
    private const string DefaultSortField = "EnglishName";
    public async Task<PaginatedResult<GetAllProductsForSpecificSellerResponse>> Handle(
        GetAllProductsForSpecificSellerQuery request,
        CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId != request.SellerId)
        {
            return await PaginatedResult<GetAllProductsForSpecificSellerResponse>.FailureAsync(
                message:"The Current user is unauthorized for that action.",
                statusCode:HttpStatusCode.Unauthorized);
        }
        var query = unitOfWork.ProductsRepo.Entities
            .Where(p => p.CreatedBy == userId);

        var sortField = request.Pagination.SortBy ?? DefaultSortField;
        var sortOrder = request.Pagination.SortOrder;
        query = query.OrderBy($"{sortField} {sortOrder}");

        return await query
            .ProjectToType<GetAllProductsForSpecificSellerResponse>()
            .ToPaginatedListAsync(
                request.Pagination.PageNumber,
                request.Pagination.PageSize,
                cancellationToken,
                "Products retrieved successfully");
    }
}