using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.colors.Queries;

public sealed record GetAllColorsQuery(string UserId) : IRequest<Result<List<GetAllColorsResponse>>>;

public sealed record GetAllColorsResponse(
    Guid ColorId,
    string Name,
    string HexCode);