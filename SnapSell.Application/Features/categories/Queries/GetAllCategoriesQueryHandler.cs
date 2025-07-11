using System.Net;
using Mapster;
using MediatR;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.categories.Queries;

internal sealed class GetAllCategoriesQueryHandler(ISQLBaseRepo<Category> categoriesRepository)
    : IRequestHandler<GetAllCategoriesQuery, Result<List<GetAllCategoriesResponse>>>
{
    public async Task<Result<List<GetAllCategoriesResponse>>> Handle(GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await categoriesRepository.GetAllAsync();
        return Result<List<GetAllCategoriesResponse>>.Success(
            data: categories.Adapt<List<GetAllCategoriesResponse>>(),
            message: "Categories returned successfully.",
            HttpStatusCode.OK);
    }
}