using SnapSell.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.product.Commands.CreateProduct;
using SnapSell.Application.DTOs.Product;

namespace SnapSell.Presentation.EndPoints;

public sealed class ProductController(ICacheService cacheService, ISender sender) : ApiControllerBase(cacheService)
{
    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request,CancellationToken cancellationToken)
    {
        var command = new CreatProductCommand(request.EnglishName,
            request.ArabicName,
            request.Description,
            request.ShortDescription,
            request.IsFeatured,
            request.IsHidden,
            request.MinDeleveryDays,
            request.MaxDeleveryDays,
            request.MainImageUrl!);

        var result = await sender.Send(command,cancellationToken);
        return HandleMediatorResult<CreateProductResponse>(result);
    }
}