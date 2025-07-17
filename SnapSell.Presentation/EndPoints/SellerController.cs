using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.products.Commands.AddAdditionalInformationToProduct;
using SnapSell.Application.Features.products.Commands.CreateProduct;
using SnapSell.Application.Features.products.Queries.GetAllProductsForSpecificSeller;
using SnapSell.Application.Features.store.Commands.CreateStore;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints;

[Authorize(Roles = "Seller")]
public sealed class SellerController(ISender sender) : ApiControllerBase
{
    [HttpPost("CreateStore")]
    public async Task<ActionResult<Result<CreateStoreResponse>>> CreateStore([FromBody] CreateStoreCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }

    [HttpGet("GetAllProductsForSpecificSeller/{sellerId}")]
    public async Task<ActionResult<PaginatedResult<GetAllProductsForSpecificSellerResponse>>>
        GetAllProductsForSpecificSeller(string sellerId, [FromQuery] PaginatedRequest request,
            CancellationToken cancellationToken)
    {
        var query = new GetAllProductsForSpecificSellerQuery(sellerId, request);
        var result = await sender.Send(query, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }

    [HttpPost("CreateProduct")]
    [RequestSizeLimit(500 * 1024 * 1024)]
    public async Task<ActionResult<Result<CreateProductResponse>>> CreateProduct([FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }

    [HttpPost("CreateProductAdditionalInformation")]
    public async Task<ActionResult<Result<CreateProductAdditionalInformationResponse>>>
        CreateProductAdditionalInformation([FromBody] AddAdditionalInformationToProductCommand command,
            CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }
}