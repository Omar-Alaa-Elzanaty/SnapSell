using Microsoft.AspNetCore.Http;

namespace SnapSell.Application.DTOs.Product;

public sealed record CreateProductResponse(Guid ProductId,
    string SellerId, 
    string EnglishName,
    string ArabicName, 
    string? Description,
    string? ShortDescription, 
    bool IsFeatured,
    bool IsHidden,
    int MinDeleveryDays,
    int MaxDeleveryDays,
    string? MainImageUrl);


public sealed record CreateProductRequest(
    string EnglishName,
    string ArabicName,
    string? Description,
    string? ShortDescription,
    bool IsFeatured,
    bool IsHidden,
    int MinDeleveryDays,
    int MaxDeleveryDays,
    IFormFile? MainImageUrl);