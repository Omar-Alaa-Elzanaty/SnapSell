using MediatR;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.Products.Queries.ProductSearch;

public sealed record SearchProductsQuery(string SearchText) 
    : PaginatedRequest, IRequest<PaginatedResult<SearchResponse>>;


public sealed class SearchResponse
{ 
    public ProductSearchDto? Product { get; set; }
    public BrandDto? Brand { get; set; }
    public CategoriesDto? Categorie { get; set; }
}

public sealed class ProductSearchDto
{
    public Guid ProductId { get; set; }
    public Guid StoreId { get; set; }
    public string EnglishName { get; set; }
    public string ArabicName { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsHidden { get; set; }
    public ShippingType ShippingType { get; set; }
    public ProductStatus ProductStatus { get; set; }
    public List<PaymentMethod> PaymentMethods { get; set; } = [];
    public List<ProductImageResponse> Images { get; set; } = [];
    public string EnglishDescription { get; set; }
    public string ArabicDescription { get; set; }
    public int MinDeliveryDays { get; set; }
    public int MaxDeliveryDays { get; set; }
    public decimal? Price { get; set; }
    public decimal? SalePrice { get; set; }
    public decimal? CostPrice { get; set; }
    public int? Quantity { get; set; }
    public string? Sku { get; set; }
    public List<ProductVariantSearchResponse>? Variants { get; set; }
}

public sealed class ProductImageResponse
{
    public string? ImageUrl { get; set; }
    public bool IsMainImage { get; set; }
}

public sealed record BrandDto(Guid BrandId, string Name);

public sealed record CategoriesDto(
    Guid CategoryId,
    string Name,
    Guid? ParentCategoryId);

public sealed record ProductVariantSearchResponse(
    Guid Id,
    Guid SizeId,
    string? Color,
    int? Quantity,
    decimal? Price,
    decimal? SalePrice,
    decimal? CostPrice,
    string? Sku,
    bool IsDefault);