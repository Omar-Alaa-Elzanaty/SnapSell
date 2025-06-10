using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Net;
using System.Security.Claims;
using Mapster;
using SnapSell.Domain.Models.SqlEntities;


namespace SnapSell.Application.Features.brands.Queries;

internal sealed class GetAllPrandsQueryHandler(
    ISQLBaseRepo<Brand> brandaRepository,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<GetAllPrandsQuery, Result<List<GetAllBrandsResponse>>>
{
    public async Task<Result<List<GetAllBrandsResponse>>> Handle(GetAllPrandsQuery request,
        CancellationToken cancellationToken)
    {
        var brands = await brandaRepository.GetAllAsync();
        return Result<List<GetAllBrandsResponse>>.Success(
            data: brands.Adapt<List<GetAllBrandsResponse>>(),
            message: "Brands returned Successfully.",
            HttpStatusCode.OK);
    }
}