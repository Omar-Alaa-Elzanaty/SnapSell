using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.categories.Queries;

public sealed record GetAllCategoriesQuery(string UserId) : IRequest<Result<List<GetAllCategoriesResponse>>>;

public sealed record GetAllCategoriesResponse(
    Guid CategoryId,
    string Name,
    Guid? ParentCategoryId);