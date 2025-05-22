using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models;
using System.Net;
using System.Security.Claims;
using Mapster;


namespace SnapSell.Application.Features.brands.Queries;

internal sealed class GetAllPrandsQueryHandler(
    ISQLBaseRepo<Brand> brandaRepository,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<GetAllPrandsQuery, Result<List<GetAllBrandsResponse>>>
{
    public async Task<Result<List<GetAllBrandsResponse>>> Handle(GetAllPrandsQuery request,
        CancellationToken cancellationToken)
    {
        var currentUser = httpContextAccessor.HttpContext?.User;
        if (currentUser is null)
        {
            return Result<List<GetAllBrandsResponse>>.Failure(
                message: "Current user is null",
                HttpStatusCode.Unauthorized);
        }

        var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<List<GetAllBrandsResponse>>.Failure(
                message: "User ID not found in claims",
                HttpStatusCode.Unauthorized);
        }

        var brands = await brandaRepository.GetAllAsync();

        return Result<List<GetAllBrandsResponse>>.Success(
            data: brands.Adapt<List<GetAllBrandsResponse>>(),
            message: "Brands returned Successfully.",
            HttpStatusCode.OK);
    }
}