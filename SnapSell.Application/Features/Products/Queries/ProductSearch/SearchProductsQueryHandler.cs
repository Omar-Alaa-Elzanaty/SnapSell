using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Abstractions.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.Products.Queries.ProductSearch;

internal sealed class SearchProductsQueryHandler(
    ISQLBaseRepo<Product> productRepository,
    ISQLBaseRepo<Brand> brandRepository,
    ISQLBaseRepo<Category> categoryRepository,
    IMediaService mediaService)
    : IRequestHandler<SearchProductsQuery, PaginatedResult<SearchResponse>>
{
    public async Task<PaginatedResult<SearchResponse>> Handle(
        SearchProductsQuery request,
        CancellationToken cancellationToken)
    {
        var searchText = request.SearchText.Trim();

        var productMatches = await productRepository.Entities
            .Where(p =>
                EF.Functions.Like(p.EnglishName, $"%{searchText}%") ||
                EF.Functions.Like(p.ArabicName, $"%{searchText}%"))
            .AsNoTracking()
            .Select(p => new SearchResponse
            {
                Product = p.Adapt<ProductSearchDto>()
            })
            .ToListAsync(cancellationToken);
        
        var brandMatches = await brandRepository.Entities
            .Where(b => EF.Functions.Like(b.Name, $"%{searchText}%"))
            .AsNoTracking()
            .Select(b => new SearchResponse
            {
                Brand = b.Adapt<BrandDto>()
            })
            .ToListAsync(cancellationToken);

        var categoryMatches = await categoryRepository.Entities
            .Where(c => EF.Functions.Like(c.Name, $"%{searchText}%"))
            .AsNoTracking()
            .Select(c => new SearchResponse
            {
                Categories = c.Adapt<CategoriesDto>()
            })
            .ToListAsync(cancellationToken);

        var allResults = productMatches
            .Concat(brandMatches)
            .Concat(categoryMatches)
            .ToList();

        var totalCount = allResults.Count;
        
        var pagedResults = allResults
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();
        
        foreach (var item in pagedResults)
        {
            if (item.Product?.Images != null)
            {
                foreach (var img in item.Product.Images)
                {
                    if (!string.IsNullOrEmpty(img.ImageUrl))
                    {
                        img.ImageUrl = mediaService.GetUrl(img.ImageUrl, MediaTypes.Image);
                    }
                }
            }
        }
        
        return await PaginatedResult<SearchResponse>.SuccessAsync(
            pagedResults,
            totalCount,
            request.PageNumber,
            request.PageSize,
            message: "Search results retrieved successfully.");
    }
}