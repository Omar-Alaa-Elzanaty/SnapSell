using MediatR;
using SnapSell.Application.DTOs.variant;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.product.Commands.AddVariantsToProduct;

public sealed record AddVariantsToProductCommand(
    Guid ProductId,
    Guid? SizeId,
    Guid? ColorId,
    int Quantity,
    decimal Price,
    decimal RegularPrice,
    decimal? SalePrice,
    string? SKU,
    string? Barcode) : IRequest<Result<List<AddVariantsToProductResponse>>>;

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