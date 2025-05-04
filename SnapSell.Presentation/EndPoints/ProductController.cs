using SnapSell.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.product.Commands.CreateProduct;
using SnapSell.Application.DTOs.Product;
using SnapSell.Application.DTOs.Brands;
using SnapSell.Application.Features.brands.Queries;
using SnapSell.Application.Features.categories.Queries;
using SnapSell.Application.DTOs.categories;
using SnapSell.Application.Features.colors.Queries;
using SnapSell.Application.DTOs.colors;

namespace SnapSell.Presentation.EndPoints;

public sealed class ProductController(ICacheService cacheService, ISender sender) : ApiControllerBase(cacheService)
{
    [HttpGet("GetAllBrands/{userId}")]
    public async Task<IActionResult> GetAllBrands(string userId, CancellationToken cancellationToken)
    {
        var query = new GetAllPrandsQuery(userId);

        var result = await sender.Send(query, cancellationToken);
        return HandleMediatorResult<List<GetAllBrandsResponse>>(result);
    }

    [HttpGet("GetAllCategories/{userId}")]
    public async Task<IActionResult> GetAllCategories(string userId, CancellationToken cancellationToken)
    {
        var query = new GetAllCategoriesQuery(userId);

        var result = await sender.Send(query, cancellationToken);
        return HandleMediatorResult<List<GetAllCategoriesResponse>>(result);
    }


    [HttpGet("GetAllColors/{userId}")]
    public async Task<IActionResult> GetAllColors(string userId, CancellationToken cancellationToken)
    {
        var query = new GetAllColorsQuery(userId);

        var result = await sender.Send(query, cancellationToken);
        return HandleMediatorResult<List<GetAllColorsResponse>>(result);
    }


    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreatProductCommand(request.EnglishName,
            request.ArabicName,
            request.Description,
            request.ShortDescription,
            request.IsFeatured,
            request.IsHidden,
            request.MinDeleveryDays,
            request.MaxDeleveryDays,
            request.MainImageUrl!,
            request.MainImageUrl!,
            request.Variants);

        var result = await sender.Send(command, cancellationToken);
        return HandleMediatorResult<CreateProductResponse>(result);
    }
}