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