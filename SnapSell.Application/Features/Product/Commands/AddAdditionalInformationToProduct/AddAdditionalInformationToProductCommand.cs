using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.product.Commands.AddAdditionalInformationToProduct;

public sealed record AddAdditionalInformationToProductCommand(
    Guid ProductId,
    string EnglishDescription,
    string ArabicDescription,
    int MinDeleveryDays,
    int MaxDeleveryDays,
    List<VariantDto?> Variants) : IRequest<Result<CreateProductAdditionalInformationResponse>>;

public sealed record CreateProductAdditionalInformationResponse(
    Guid ProductId,
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

public sealed record VariantDto(
    string? Size,
    string? Color,
    int Quantity,
    decimal Price,
    decimal RegularPrice,
    decimal? SalePrice,
    string? Sku);


public sealed record VariantResponse(
    Guid VariantId,
    Guid? SizeId,
    Guid? ColorId,
    int Quantity,
    decimal Price,
    decimal RegularPrice,
    decimal? SalePrice,
    string? Sku,
    string? Barcode
);