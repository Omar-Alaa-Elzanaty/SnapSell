using SnapSell.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.Product.Commands.CreateProduct;

namespace SnapSell.Presentation.EndPoints;

public sealed class ProductController(ICacheService cacheService, ISender sender) : ApiControllerBase(cacheService)
{
    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProduct()
    {
        var command = new CreatProductCommand();
        var result = await sender.Send(command);
        return Ok(result);
    }
}