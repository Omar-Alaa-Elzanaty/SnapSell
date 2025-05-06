using Microsoft.AspNetCore.Http;
using SnapSell.Application.DTOs.variant;

namespace SnapSell.Application.DTOs.Product;

public sealed record CreateProductResponse(
    Guid ProductId,
    string SellerId,
    string EnglishName,
    string ArabicName,
    string? Description,
    string? ShortDescription,
    bool IsFeatured,
    bool IsHidden,
    int MinDeleveryDays,
    int MaxDeleveryDays,
    string? MainImageUrl,
    string? MainVideoUrl,
    List<VariantResponse> Variants);

public sealed record CreateProductRequest(
    Guid BrandId,
    string EnglishName,
    string ArabicName,
    string? Description,
    string? ShortDescription,
    bool IsFeatured,
    bool IsHidden,
    int MinDeleveryDays,
    int MaxDeleveryDays,
    IFormFile? MainImageUrl,
    IFormFile? MainVideoUrl);