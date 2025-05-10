using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SnapSell.Application.Extensions;
using System.Net;
using System.Security.Claims;
using SnapSell.Application.DTOs.Product;

namespace SnapSell.Application.Features.product.Queries.GetAllProductsForSpecificSeller;

internal sealed class GetAllProductsForSpecificSellerQueryHandler(
    IHttpContextAccessor httpContextAccessor,
    ISQLBaseRepo<Product> productRepository) : IRequestHandler<GetAllProductsForSpecificSellerQuery
    , PaginatedResult<GetAllProductsForSpecificSellerResponse>>
{
    public async Task<PaginatedResult<GetAllProductsForSpecificSellerResponse>> Handle(
        GetAllProductsForSpecificSellerQuery request, CancellationToken cancellationToken)
    {
        var currentUser = httpContextAccessor.HttpContext?.User;
        if (currentUser is null)
        {
            return await PaginatedResult<GetAllProductsForSpecificSellerResponse>.FailureAsync(
                message: "Current user is null",
                statusCode: HttpStatusCode.Unauthorized);
        }

        var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return await PaginatedResult<GetAllProductsForSpecificSellerResponse>.FailureAsync(
                message: "User ID not found in claims",
                statusCode: HttpStatusCode.Unauthorized);
        }

        var query = productRepository.TheDbSet()
            .Where(p => !p.IsHidden)
            .Include(p => p.Brand)
            .Include(p => p.Variants)
            .AsQueryable();

        var paginatedProducts = await query
            .OrderBy(p => p.EnglishName)
            .ProjectToType<GetAllProductsForSpecificSellerResponse>()
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

        return paginatedProducts;
    }
}