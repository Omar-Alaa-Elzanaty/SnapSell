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

        if (string.IsNullOrEmpty(userId))
            return await PaginatedResult<GetAllProductsForSpecificSellerResponse>.FailureAsync(
                "Authentication required", HttpStatusCode.Unauthorized);

        if (userId != request.SellerId)
            return await PaginatedResult<GetAllProductsForSpecificSellerResponse>.FailureAsync(
                "You can only view your own products", HttpStatusCode.Forbidden);
        try
        {
            var query = productRepository.TheDbSet()
                .Where(p => !p.IsHidden && p.CreatedBy == userId);


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
        catch (ArgumentException ex) when (ex.Message.Contains("sorting", StringComparison.OrdinalIgnoreCase))
        {
            return await PaginatedResult<GetAllProductsForSpecificSellerResponse>.FailureAsync(
                message:
                $"Invalid sorting parameter: {ex.Message}, Allowed is: IsFeatured , IsHidden, EnglishName, ArabicName.",
                statusCode: HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            return await PaginatedResult<GetAllProductsForSpecificSellerResponse>.FailureAsync(
                $"An error occurred: {ex.Message}",
                HttpStatusCode.InternalServerError);
        }
    }
}