using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Products.Commands.UpdateVariantById;

public sealed record UpdateVariantCommand(
    Guid VariantId,
    string Name,
    decimal Price,
    decimal SalePrice,
    decimal CostPrice,
    int Quantity,
    Guid SizeId) : IRequest<Result<Unit>>;