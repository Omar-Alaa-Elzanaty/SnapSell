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
using SnapSell.Domain.Models.MongoDbEntities;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.Products.Queries.ProductSearch;

internal sealed class SearchProductsQueryHandler(
    IMongoCollection<Product> productCollection,
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

        var products = await productCollection.TextSearchAsync(
            searchText: request.SearchText,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);

        var response = new SearchResponse
        {
            Products = products.Adapt<List<ProductSearchDto>>(),
            Brands = new List<BrandDto>(),
            Categories = new List<CategoriesDto>()
        };

        foreach (var product in response.Products)
        {
            foreach (var image in product.Images)
            {
                image.ImageUrl = mediaService.GetUrl(image.ImageUrl, MediaTypes.Image);
            }
        }

        var brands = await brandRepo.Entities
            .Where(b => b.Name.Contains(request.SearchText))
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        response.Brands = brands.Adapt<List<BrandDto>>();
        
        var categories = await categoryRepo.Entities
            .Where(c => c.Name.Contains(request.SearchText))
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        response.Categories = categories.Adapt<List<CategoriesDto>>();
        
        var filter = Builders<Product>.Filter.Text(request.SearchText);
        var totalCount = (int)await productCollection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return await PaginatedResult<SearchResponse>.SuccessAsync(
            new List<SearchResponse> { response },
            totalCount,
            request.PageNumber,
            request.PageSize,
            isArabic ? "تم العثور على النتائج" : "Search results found");
    }
}