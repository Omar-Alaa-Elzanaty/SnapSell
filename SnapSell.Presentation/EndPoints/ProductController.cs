using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.brands.Queries;
using SnapSell.Application.Features.categories.Queries;
using SnapSell.Application.Features.product.Commands.AddAdditionalInformationToProduct;
using SnapSell.Application.Features.product.Commands.CreateProduct;
using SnapSell.Application.Features.product.Queries.GetAllProductsForSpecificSeller;
using SnapSell.Application.Features.product.Queries.GetSizes;
using SnapSell.Domain.Dtos;
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
        return await HandleMediatorResult(result);
    }

    [HttpGet("GetAllCategories")]
    public async Task<ActionResult<Result<List<GetAllCategoriesResponse>>>> GetAllCategories(CancellationToken cancellationToken)
    {
        var query = new GetAllCategoriesQuery();

        var result = await sender.Send(query, cancellationToken);
        return await HandleMediatorResult(result);
    }

    [HttpGet("GetAllSizes")]
    public async Task<ActionResult<Result<IReadOnlyList<GetAllSizesResponse>>>> GetAllSizes(CancellationToken cancellationToken)
    {
        var query = new GetAllSizesQuery();
        var result = await sender.Send(query, cancellationToken);
        return await HandleMediatorResult(result);
    }

    [HttpPost("CreateProduct")]
    [RequestSizeLimit(500 * 1024 * 1024)] // 500MB limit
    public async Task<ActionResult<Result<CreateProductResponse>>> CreateProduct([FromBody] CreatProductCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResult(result);
    }

    [HttpPost("CreateProductAdditionalInformation")]
    public async Task<ActionResult<Result<CreateProductAdditionalInformationResponse>>> CreateProductAdditionalInformation(
        [FromBody] AddAdditionalInformationToProductCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResult(result);
    }

    [HttpGet("GetAllProductsForSpecificSeller/{sellerId}")]
    public async Task<ActionResult<PaginatedResult<GetAllProductsForSpecificSellerResponse>>> GetAllProductsForSpecificSeller(string sellerId,
        [FromQuery] PaginatedRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetAllProductsForSpecificSellerQuery(sellerId, request);

        var result = await sender.Send(query, cancellationToken);
        return await HandleMediatorResult(result);
    }
}