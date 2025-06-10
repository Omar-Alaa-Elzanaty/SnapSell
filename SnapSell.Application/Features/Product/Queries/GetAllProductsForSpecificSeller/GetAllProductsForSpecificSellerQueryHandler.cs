using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Extensions;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.MongoDbEntities;
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

        var query = productRepository.TheDbSet()
            .Where(p => p.CreatedBy == userId);

        var sortField = request.Pagination.SortBy ?? DefaultSortField;
        var sortOrder = request.Pagination.SortOrder ?? DefaultOrderField;
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