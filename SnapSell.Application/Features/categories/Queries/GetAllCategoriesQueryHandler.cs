using System.Net;
using Mapster;
using MediatR;
using SnapSell.Application.Abstractions.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.categories.Queries;

internal sealed class GetAllCategoriesQueryHandler(ISQLBaseRepo<Category> categoriesRepository)
    : IRequestHandler<GetAllCategoriesQuery, Result<List<GetAllCategoriesGroupedResponse>>>
{
    public async Task<Result<List<GetAllCategoriesGroupedResponse>>> Handle(GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await categoriesRepository.GetAllAsync();
        var categoriesDto = categories.Adapt<List<GetAllCategoriesResponse>>();
        var inMemoryCategories = categoriesDto
            .ToDictionary(x => x.CategoryId);
        
        var grouped = categoriesDto
            .Where(s => s.ParentCategoryId != null)
            .GroupBy(s => s.ParentCategoryId!.Value)
            .Select(group =>
            {
                var parent = inMemoryCategories.GetValueOrDefault(group.Key);
                return new GetAllCategoriesGroupedResponse(parent, group.ToList());
            }).ToList();
        
        return Result<List<GetAllCategoriesGroupedResponse>>.Success(
            data:grouped,
            message: "Categories returned successfully.",
            HttpStatusCode.OK);
    }
}