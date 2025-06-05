using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.brands.Queries;
using SnapSell.Application.Features.categories.Queries;
using SnapSell.Application.Features.product.Commands.AddAdditionalInformationToProduct;
using SnapSell.Application.Features.product.Commands.CreateProduct;
using SnapSell.Application.Features.product.Queries.GetAllPaymentMethods;
using SnapSell.Application.Features.product.Queries.GetAllProductsForSpecificSeller;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints;

public sealed class ProductController(ICacheService cacheService, ISender sender) : ApiControllerBase(cacheService)
{
    [HttpGet("GetAllBrands/{sellerId}")]
    public async Task<ActionResult<Result<List<GetAllBrandsResponse>>>> GetAllBrands(string sellerId, CancellationToken cancellationToken)
    {
        var query = new GetAllPrandsQuery(sellerId);

        var result = await sender.Send(query, cancellationToken);
        return await HandleMediatorResult(result);
    }

    [HttpGet("GetAllCategories/{userId}")]
    public async Task<ActionResult<Result<List<GetAllCategoriesResponse>>>> GetAllCategories(string userId, CancellationToken cancellationToken)
    {
        var query = new GetAllCategoriesQuery(userId);

        var result = await sender.Send(query, cancellationToken);
        return await HandleMediatorResult(result);
    }

    [HttpGet("GetAllPaymentMethods/{userId}")]
    public async Task<ActionResult<Result<List<GetAllPaymentMethodsResponse>>>> GetAllPaymentMethods(string userId, CancellationToken cancellationToken)
    {
        var query = new GetAllPaymentMethodsQuery(userId);

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