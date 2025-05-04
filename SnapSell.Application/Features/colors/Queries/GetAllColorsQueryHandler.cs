using System.Drawing;
using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.DTOs.colors;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Net;
using System.Security.Claims;
using Mapster;
using SnapSell.Application.Interfaces.Repos;

namespace SnapSell.Application.Features.colors.Queries;

internal sealed class GetAllColorsQueryHandler(
    IHttpContextAccessor httpContextAccessor,
    ISQLBaseRepo<Color> colorRepository) : IRequestHandler<GetAllColorsQuery, Result<List<GetAllColorsResponse>>>
{
    public async Task<Result<List<GetAllColorsResponse>>> Handle(GetAllColorsQuery request,
        CancellationToken cancellationToken)
    {
        var currentUser = httpContextAccessor.HttpContext?.User;
        if (currentUser is null)
        {
            return Result<List<GetAllColorsResponse>>.Failure(
                message: "Current user is null",
                HttpStatusCode.Unauthorized);
        }

        var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<List<GetAllColorsResponse>>.Failure(
                message: "User ID not found in claims",
                HttpStatusCode.Unauthorized);
        }

        var colors = await colorRepository.GetAllAsync();

        return Result<List<GetAllColorsResponse>>.Success(
            data: colors.Adapt<List<GetAllColorsResponse>>(),
            message: "Colors returned Successfully.",
            HttpStatusCode.OK);
    }
}