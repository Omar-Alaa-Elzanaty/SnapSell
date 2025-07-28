using MediatR;
using SnapSell.Application.Features.products.Commands.CreateProduct;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.Products.Commands.UpdateProductBasicInfo;

public sealed record UpdateProductBasicInfoCommand(
    int ProductId,
    Guid BrandId,
    List<Guid> CategoryIds,
    string EnglishName,
    string ArabicName,
    bool IsFeatured,
    bool IsHidden,
    ProductStatus ProductStatus,
    bool HasVariants,
    int ShippingType,
    ProductTypes ProductType,
    List<int> PaymentMethods,
    List<ProductImageDto> Images,
    string EnglishDescription,
    string ArabicDescription,
    int MinDeliveryDays,
    int MaxDeliveryDays,
    decimal? Price,
    decimal? SalePrice,
    decimal? CostPrice,
    int? Quantity,
    string? Sku ) : IRequest<Result<UpdateProductBasicInfoResponse>>;


public sealed class UpdateProductBasicInfoResponse
{
    public int ProductId { get; set; }
    public Guid StoreId { get; set; }
    public string EnglishName { get; set; }
    public string ArabicName { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsHidden { get; set; }
    public ShippingType ShippingType { get; set; }
    public ProductTypes ProductStatus { get; set; }
    public List<PaymentMethods> PaymentMethods { get; set; } = [];
    public List<UpdateProductImageResponse> Images { get; set; } = [];
    public string EnglishDescription { get; set; }
    public string ArabicDescription { get; set; }
    public int MinDeliveryDays { get; set; }
    public int MaxDeliveryDays { get; set; }
    public decimal? Price { get; set; }
    public decimal? SalePrice { get; set; }
    public decimal? CostPrice { get; set; }
    public int? Quantity { get; set; }
    public string? Sku { get; set; }
}

public sealed class UpdateProductImageResponse
{
    public string? ImageUrl { get; set; }
    public bool IsMainImage { get; set; }
}