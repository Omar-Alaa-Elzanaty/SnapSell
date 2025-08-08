using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.Products.Commands.ClearProductVariants;
using SnapSell.Application.Features.products.Commands.CreateProduct;
using SnapSell.Application.Features.Products.Commands.DeleteProduct;
using SnapSell.Application.Features.Products.Commands.UpdateProductBasicInfo;
using SnapSell.Application.Features.Products.Commands.UpdateProductVariants;
using SnapSell.Application.Features.Products.Commands.UpdateVariantById;
using SnapSell.Application.Features.products.Queries.GetAllProductsForSpecificSeller;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Constants;

namespace SnapSell.Presentation.EndPoints;

[Authorize(Roles = Roles.Seller)]
public sealed class SellerController(ISender sender) : ApiControllerBase
{
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
    public async Task<ActionResult<Result<CreateProductResponse>>> CreateProduct(
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }

    [HttpPut("UpdateProductBasicInfo")]
    public async Task<ActionResult<Result<UpdateProductBasicInfoResponse>>> UpdateBasicInfo(
        [FromBody]UpdateProductBasicInfoCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }

    [HttpPut("UpdateProductVariants")]
    public async Task<ActionResult<Result<UpdateProductVariantsResponse>>> UpdateVariants(
        [FromBody]UpdateProductVariantsCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }
    
    [HttpDelete("DeleteProduct")]
    public async Task<ActionResult<Result<Unit>>> DeleteProduct(
        [FromBody]DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }
    
    [HttpDelete("ClearProductVariants")]
    public async Task<ActionResult<Result<Unit>>> ClearProductVariants(
        [FromQuery]ClearProductVariantsCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }
    
    [HttpPut("UpdateVariantById")]
    public async Task<ActionResult<Result<Unit>>> UpdateVariantById(
        [FromBody]UpdateVariantCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }
}