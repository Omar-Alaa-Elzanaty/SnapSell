using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Extensions;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.Products.Queries.ProductSearch;

internal sealed class SearchProductsQueryHandler(
    ISQLBaseRepo<Product> productRepository,
    ISQLBaseRepo<Brand> brandRepo,
    ISQLBaseRepo<Category> categoryRepo,
    IMediaService mediaService,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<SearchProductsQuery, PaginatedResult<SearchResponse>>
{
    public async Task<PaginatedResult<SearchResponse>> Handle(
        SearchProductsQuery request,
        CancellationToken cancellationToken)
    {
        var language = httpContextAccessor.HttpContext?
            .Request
            .GetTypedHeaders()
            .AcceptLanguage
            .FirstOrDefault()?
            .Value.ToString() ?? "en";

        var isArabic = language.StartsWith("ar");

        var query = productRepository.Entities
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Where(p =>
                EF.Functions.Like(p.EnglishName, $"%{request.SearchText}%") ||
                EF.Functions.Like(p.ArabicName, $"%{request.SearchText}%") ||
                EF.Functions.Like(p.EnglishDescription, $"%{request.SearchText}%") ||
                EF.Functions.Like(p.ArabicDescription, $"%{request.SearchText}%") ||
                EF.Functions.Like(p.Brand.Name, $"%{request.SearchText}%") ||
                EF.Functions.Like(p.Category.Name, $"%{request.SearchText}%"));
        
        
        var count = await query.CountAsync(cancellationToken);

        var products = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var result = new List<SearchResponse>();

        foreach (var product in products)
        {
            var response = product.Adapt<SearchResponse>();
            if (response.Product?.Images is not null)
            {
                foreach (var img in response.Product.Images)
                {
                    if (!string.IsNullOrWhiteSpace(img.ImageUrl))
                        img.ImageUrl = mediaService.GetUrl(img.ImageUrl,MediaTypes.Image);
                }
            }

            result.Add(response);
        }

        return await PaginatedResult<SearchResponse>.SuccessAsync(
            items: result,
            totalCount: count,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize);
    }
}