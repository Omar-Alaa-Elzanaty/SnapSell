using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SnapSell.Application.DTOs.Product;
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
        try
        {
            var query = productRepository.TheDbSet()
                .Where(p => !p.IsHidden && p.CreatedBy == userId)
                .Include(p => p.Brand)
                .Include(p => p.Variants)
                .AsNoTracking();

            var sortExpression = $"{request.Pagination.SortBy ?? DefaultSortField} {request.Pagination.SortOrder}";
            query = query.OrderBy(sortExpression);

            var result = await query
                .Select(p => p)
                .ToPaginatedListAsync(
                    request.Pagination.PageNumber,
                    request.Pagination.PageSize,
                    cancellationToken);

            var mappedData = result.Data.Adapt<List<GetAllProductsForSpecificSellerResponse>>();
            return new PaginatedResult<GetAllProductsForSpecificSellerResponse>(mappedData, result.TotalCount,
                result.CurrentPage, result.PageSize)
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Products retrieved successfully"
            };
        }
        catch (ArgumentException ex) when (ex.Message.Contains("sorting", StringComparison.OrdinalIgnoreCase))
        {
            return await PaginatedResult<GetAllProductsForSpecificSellerResponse>.FailureAsync(
                $"Invalid sorting parameter: {ex.Message}, Allowwed is: ASC , DESC.",
                HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            return await PaginatedResult<GetAllProductsForSpecificSellerResponse>.FailureAsync(
                $"An error occurred: {ex.Message}",
                HttpStatusCode.InternalServerError);
        }
    }
}