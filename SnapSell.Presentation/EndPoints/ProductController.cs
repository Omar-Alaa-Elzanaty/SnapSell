using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.brands.Queries;
using SnapSell.Application.Features.categories.Queries;
using SnapSell.Application.Features.products.Queries.GetSizes;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints;

[Authorize(Roles = "Seller")]
public sealed class ProductController(ISender sender) : ApiControllerBase
{
    [HttpGet("GetAllBrands")]
    public async Task<ActionResult<Result<List<GetAllBrandsResponse>>>> GetAllBrands(CancellationToken cancellationToken)
    {
        var query = new GetAllPrandsQuery();
        var result = await sender.Send(query, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }

    [HttpGet("GetAllCategories")]
    public async Task<ActionResult<Result<List<GetAllCategoriesResponse>>>> GetAllCategories(CancellationToken cancellationToken)
    {
        var query = new GetAllCategoriesQuery();
        var result = await sender.Send(query, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }

    [HttpGet("GetAllSizes")]
    public async Task<ActionResult<Result<IReadOnlyList<GetAllSizesGroupedResponse>>>> GetAllSizes(CancellationToken cancellationToken)
    {
        var query = new GetAllSizesQuery();
        var result = await sender.Send(query, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }

}