using Microsoft.AspNetCore.Http;
using SnapSell.Application.DTOs.variant;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.DTOs.Product;

public sealed record CreateProductResponse(
    Guid ProductId,
    string SellerId,
    string EnglishName,
    string ArabicName,
    bool IsFeatured,
    bool IsHidden,
    ShippingType ShippingType,
    ProductStatus ProductStatus);

public sealed record CreateProductAdditionalInformationResponse(
    Guid ProductId,
    string SellerId,
    string EnglishName,
    string ArabicName,
    string EnglishDescription,
    string ArabicDescription,
    bool IsFeatured,
    bool IsHidden,
    int MinDeleveryDays,
    int MaxDeleveryDays,
    string? MainImageUrl,
    List<VariantResponse> Variants);

public sealed record CreateProductAdditionalInformationRequest(
    Guid ProductId,
    string EnglishDescription,
    string ArabicDescription,
    int MinDeleveryDays,
    int MaxDeleveryDays,
    List<VariantDto?> Variants);

public sealed record CreateProductRequest(
    Guid BrandId,
    string EnglishName,
    string ArabicName,
    bool IsFeatured,
    bool IsHidden,
    ShippingType ShippingType,
    ProductStatus ProductStatus);

public sealed record UploadVideoRequest(Guid ProductId, IFormFile Video);

public sealed record UploeadProductVideoResponse(string? FullVideoUrl);