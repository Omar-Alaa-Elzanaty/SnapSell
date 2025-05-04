using MediatR;
using SnapSell.Application.DTOs.colors;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.colors.Queries;

public sealed record GetAllColorsQuery(string UserId) : IRequest<Result<List<GetAllColorsResponse>>>;