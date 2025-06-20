using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.product.Queries.GetSizes;

public sealed record GetAllSizesQuery() : IRequest<Result<IReadOnlyList<GetAllSizesGroupedResponse>>>;

public sealed record GetAllSizesResponse(
    Guid Id,
    string Name,
    Guid? ParentSizeId);

public sealed record GetAllSizesGroupedResponse(
    GetAllSizesResponse? Parent,
    List<GetAllSizesResponse> Children);