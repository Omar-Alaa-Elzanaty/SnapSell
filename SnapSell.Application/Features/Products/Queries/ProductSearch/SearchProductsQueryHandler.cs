using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.Products.Queries.ProductSearch;

internal sealed class SearchProductsQueryHandler(
    ISQLBaseRepo<Product> productRepository,
    IMediaService mediaService)
    : IRequestHandler<SearchProductsQuery, PaginatedResult<SearchResponse>>
{
    public async Task<PaginatedResult<SearchResponse>> Handle(
        SearchProductsQuery request,
        CancellationToken cancellationToken)
    {
        var searchText = request.SearchText.Trim();

        var query = productRepository.Entities
            .Include(p => p.Brand)
            .Include(p => p.Categories)
            .ThenInclude(pc => pc.Category)
            .Include(p => p.Images)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(p =>
                EF.Functions.Like(p.EnglishName, $"%{searchText}%") ||
                EF.Functions.Like(p.ArabicName, $"%{searchText}%") ||
                EF.Functions.Like(p.Brand.Name, $"%{searchText}%") ||
                p.Categories.Any(pc =>
                    pc.Category != null &&
                    EF.Functions.Like(pc.Category.Name, $"%{searchText}%")));
        }
        
        var totalCount = await query.CountAsync(cancellationToken);

        var products = await query
            .OrderByDescending(p => p.IsFeatured)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        
        var responseItems = products.Select(product =>
        {
            var response = new SearchResponse
            {
                Product = product.Adapt<ProductSearchDto>(),
                Brand = product.Brand.Adapt<BrandDto>(),
                Categories = product.Categories
                    .Where(pc => pc.Category != null)
                    .Select(pc => pc.Category.Adapt<CategoriesDto>())
                    .FirstOrDefault()
            };
            
            if (response.Product?.Images != null)
            {
                foreach (var img in response.Product.Images)
                {
                    if (!string.IsNullOrWhiteSpace(img.ImageUrl))
                    {
                        img.ImageUrl = mediaService.GetUrl(img.ImageUrl, MediaTypes.Image);
                    }
                }
            }
            
            return response;
        }).ToList();

        return await PaginatedResult<SearchResponse>.SuccessAsync(
            responseItems,
            totalCount,
            request.PageNumber,
            request.PageSize,
            message: "Search results retrieved successfully.");
    }
}