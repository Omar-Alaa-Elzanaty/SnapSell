using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.brands.Queries;

public sealed record GetAllPrandsQuery(string CurrentUserId) : IRequest<Result<List<GetAllBrandsResponse>>>;

public sealed record GetAllBrandsResponse(Guid BrandId, string Name);