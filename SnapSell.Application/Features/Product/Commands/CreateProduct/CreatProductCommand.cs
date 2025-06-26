using MediatR;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.product.Commands.CreateProduct;

public sealed record CreatProductCommand(Guid StoreId,
    Guid BrandId,
    List<Guid> CategoryIds,
    string EnglishName,
    string ArabicName,
    bool IsFeatured,
    bool IsHidden,
    bool HasVariants,
    int ShippingType,
    int ProductStatus,
    List<int> PaymentMethods,
    List<MediaFileDto> Images,
    string EnglishDescription,
    string ArabicDescription,
    int MinDeliveryDays,
    int MaxDeliveryDays,
    decimal? Price,
    decimal? SalePrice,
    decimal? CostPrice,
    int? Quantity,
    string? Sku,
    List<CreatProductVariantDto>? Variants) : IRequest<Result<CreateProductResponse>>;

public sealed class CreateProductResponse
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
    public List<CreateProductVariantResponse>? Variants { get; set; }
}

public sealed class ProductImageResponse
{
    public string? ImageUrl { get; set; }
    public bool IsMainImage { get; set; }
}

public sealed record CreatProductVariantDto(
    Guid SizeId,
    string Color,
    int? Quantity,
    decimal? Price,
    decimal? SalePrice,
    decimal? CostPrice,
    string? Sku,
    bool IsDefault);

public sealed record CreateProductVariantResponse(
    Guid Id,
    Guid SizeId,
    string? Color,
    int? Quantity,
    decimal? Price,
    decimal? SalePrice,
    decimal? CostPrice,
    string? Sku,
    bool IsDefault);