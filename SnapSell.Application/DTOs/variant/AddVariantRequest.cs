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