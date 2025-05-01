using System.Net;
using System.Security.Claims;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.DTOs.categories;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models;

namespace SnapSell.Application.Features.categories.Queries;

internal sealed class GetAllCategoriesQueryHandler(
    ISQLBaseRepo<Category> categoriesRepository,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<GetAllCategoriesQuery, Result<List<GetAllCategoriesResponse>>>
{
    public async Task<Result<List<GetAllCategoriesResponse>>> Handle(GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var currentUser = httpContextAccessor.HttpContext?.User;
        if (currentUser is null)
        {
            return Result<List<GetAllCategoriesResponse>>.Failure(
                message: "Current user is null",
                HttpStatusCode.Unauthorized);
        }

        var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<List<GetAllCategoriesResponse>>.Failure(
                message: "User ID not found in claims",
                HttpStatusCode.Unauthorized);
        }

        var categories = await categoriesRepository.GetAllAsync();

        return Result<List<GetAllCategoriesResponse>>.Success(
            data: categories.Adapt<List<GetAllCategoriesResponse>>(),
            message: "Categories returned successfully.",
            HttpStatusCode.OK);
    }
}