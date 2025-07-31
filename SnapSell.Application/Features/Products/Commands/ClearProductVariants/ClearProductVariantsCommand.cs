using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Products.Commands.ClearProductVariants;

public sealed record ClearProductVariantsCommand(int ProductId) : IRequest<Result<Unit>>;