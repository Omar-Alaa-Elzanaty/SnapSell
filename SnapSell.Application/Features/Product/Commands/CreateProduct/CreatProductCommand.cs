using MediatR;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.product.Commands.CreateProduct;

public sealed record CreatProductCommand(Guid BrandId,
    string EnglishName,
    string ArabicName,
    bool IsFeatured,
    bool IsHidden,
    ShippingType ShippingType,
    ProductStatus ProductStatus,
    byte[] ImageData,
    byte[] VideoData,
    string EnglishDescription,
    string ArabicDescription,
    int MinDeleveryDays,
    int MaxDeleveryDays,
    MediaFileDto Image,
    List<CreatProductVariantDto?> Variants) : IRequest<Result<CreateProductResponse>>;

public sealed record CreateProductResponse(
    Guid ProductId,
    string EnglishName,
    string ArabicName,
    bool IsFeatured,
    bool IsHidden,
    ShippingType ShippingType,
    ProductStatus ProductStatus,
    string? MainImageUrl,
    string? MainVideoUrl,
    string EnglishDescription,
    string ArabicDescription,
    int MinDeleveryDays,
    int MaxDeleveryDays,
    List<CreatProductVariantResponse?> Variants);


public sealed record CreatProductVariantDto(string? Size,
    string? Color,
    int Quantity,
    decimal Price,
    decimal RegularPrice,
    decimal? SalePrice,
    string? SKU);

public sealed record CreatProductVariantResponse(Guid Id,
    string? Size,
    string? Color,
    int Quantity,
    decimal Price,
    decimal RegularPrice,
    decimal? SalePrice,
    string? SKU);