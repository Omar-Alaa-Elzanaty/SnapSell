using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SnapSell.Application.Extensions;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models;
using System.Net;
using System.Security.Claims;

namespace SnapSell.Application.Features.product.Queries.GetAllProductsForSpecificSeller;

internal sealed class GetAllProductsForSpecificSellerQueryHandler(
    ISQLBaseRepo<Product> productRepository,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<GetAllProductsForSpecificSellerQuery, PaginatedResult<GetAllProductsForSpecificSellerResponse>>
{
    private const string DefaultSortField = "EnglishName";
    private const string DefaultOrderField = "asc";

    public async Task<PaginatedResult<GetAllProductsForSpecificSellerResponse>> Handle(
        GetAllProductsForSpecificSellerQuery request,
        CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return await PaginatedResult<GetAllProductsForSpecificSellerResponse>.FailureAsync(
                "Authentication required", HttpStatusCode.Unauthorized);

        if (userId != request.SellerId)
            return await PaginatedResult<GetAllProductsForSpecificSellerResponse>.FailureAsync(
                "You can only view your own products", HttpStatusCode.Forbidden);


        var query = productRepository.TheDbSet()
            .Where(p => !p.IsHidden && p.CreatedBy == userId)
            .Include(p => p.Brand)
            .Include(p => p.Variants)
            .AsNoTracking();

        var sortField = request.Pagination.SortBy ?? DefaultSortField;
        var sortOrder = request.Pagination.SortOrder ?? DefaultOrderField;
        query = query.OrderBy($"{sortField} {sortOrder}");

        var paginatedData = await query.ToPaginatedListAsync(
            request.Pagination.PageNumber,
            request.Pagination.PageSize,
            cancellationToken);

        var responseItems = paginatedData.Data?.Items.Adapt<List<GetAllProductsForSpecificSellerResponse>>() ?? [];

        return new PaginatedResult<GetAllProductsForSpecificSellerResponse>(
            items: responseItems,
            totalCount: paginatedData.Data!.Meta.TotalCount,
            pageNumber: paginatedData.Data.Meta.CurrentPage,
            pageSize: paginatedData.Data.Meta.PageSize,
            message: "Products retrieved successfully");
    }
}