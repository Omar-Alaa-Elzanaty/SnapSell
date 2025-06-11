using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.product.Queries.GetSizes;

public sealed record GetAllSizesQuery() : IRequest<Result<IReadOnlyList<GetAllSizesResponse>>>;

public sealed record GetAllSizesResponse(
    Guid Id,
    string Name,
    Guid? ParentSizeId);