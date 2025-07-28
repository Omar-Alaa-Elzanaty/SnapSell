using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Products.Commands.UpdateProductVariants;


public sealed record UpdateProductVariantsCommand(
    int ProductId,
    bool HasVariants,
    List<UpdateProductVariantDto> Variants
) : IRequest<Result<UpdateProductVariantsResponse>>;

public sealed record UpdateProductVariantDto(
    Guid Id,
    Guid SizeId,
    string Color,
    int? Quantity,
    decimal? Price,
    decimal? SalePrice,
    decimal? CostPrice,
    string? Sku,
    bool IsDefault);

public sealed class UpdateProductVariantsResponse
{
    public int ProductId { get; set; }
    public List<UpdateProductVariantResponse>? Variants { get; set; }
}

public sealed record UpdateProductVariantResponse(
    Guid Id,
    Guid SizeId,
    string? Color,
    int? Quantity,
    decimal? Price,
    decimal? SalePrice,
    decimal? CostPrice,
    string? Sku,
    bool IsDefault);