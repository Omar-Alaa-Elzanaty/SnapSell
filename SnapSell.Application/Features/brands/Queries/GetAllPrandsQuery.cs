using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.brands.Queries;

public sealed record GetAllPrandsQuery() : IRequest<Result<List<GetAllBrandsResponse>>>;

public sealed record BrandsResponse(Guid BrandId, string Name);

public sealed record GetAllBrandsResponse(
    BrandsResponse ParentBrand,
    List<BrandsResponse> ChildBrands);