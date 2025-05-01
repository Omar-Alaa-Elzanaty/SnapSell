using MediatR;
using SnapSell.Application.DTOs.categories;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.categories.Queries;

public sealed record GetAllCategoriesQuery(string UserId) : IRequest<Result<List<GetAllCategoriesResponse>>>;