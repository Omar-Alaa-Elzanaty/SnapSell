namespace SnapSell.Application.DTOs.variant;

public sealed record VariantDto(
    Guid? SizeId,
    Guid? ColorId,
    int Quantity,
    decimal Price,
    decimal RegularPrice,
    decimal? SalePrice,
    string? SKU,
    string? Barcode);

public sealed record VariantResponse(
    Guid VariantId,
    Guid? SizeId,
    Guid? ColorId,
    int Quantity,
    decimal Price,
    decimal RegularPrice,
    decimal? SalePrice,
    string? SKU,
    string? Barcode
);

public sealed record AddVariantsToProductResponse(Guid ProductId,
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


public sealed record VariantResponseInGetAllProductsToSeller(
    Guid VariantId,
    Guid? SizeId,
    Guid? ColorId,
    int Quantity,
    decimal Price,
    decimal RegularPrice,
    decimal? SalePrice,
    string? SKU,
    string? Barcode);