using SnapSell.Domain.Enums;

namespace SnapSell.Application.DTOs.Product;

public sealed record GetAllProductsForSpecificSellerResponse(
    Guid ProductId,
    string EnglishName,
    string ArabicName,
    string? EnglishDescription,
    string? ArabicDescription,
    bool IsFeatured,
    bool IsHidden,
    ProductStatus? ProductStatus,
    int MinDeliveryDays,
    int MaxDeliveryDays,
    string? MainImageUrl,
    string? MainVideoUrl,
    ShippingType? ShippingType,
    string BrandName,
    int VariantCount);