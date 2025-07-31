using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(int ProductId) : IRequest<Result<Unit>>;