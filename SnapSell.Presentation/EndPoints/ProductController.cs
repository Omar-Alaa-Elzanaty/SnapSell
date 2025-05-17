using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.DTOs.Brands;
using SnapSell.Application.DTOs.categories;
using SnapSell.Application.DTOs.colors;
using SnapSell.Application.DTOs.media;
using SnapSell.Application.DTOs.payment;
using SnapSell.Application.DTOs.Product;
using SnapSell.Application.Features.brands.Queries;
using SnapSell.Application.Features.categories.Queries;
using SnapSell.Application.Features.colors.Queries;
using SnapSell.Application.Features.product.Commands.AddAdditionalInformationToProduct;
using SnapSell.Application.Features.product.Commands.CreateProduct;
using SnapSell.Application.Features.product.Commands.UploadProductImage;
using SnapSell.Application.Features.product.Commands.UploadProductVideo;
using SnapSell.Application.Features.product.Queries.GetAllPaymentMethods;
using SnapSell.Application.Features.product.Queries.GetAllProductsForSpecificSeller;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos;

namespace SnapSell.Presentation.EndPoints;

public sealed class ProductController(ICacheService cacheService, ISender sender) : ApiControllerBase(cacheService)
{
    [HttpGet("GetAllBrands/{sellerId}")]
    public async Task<IActionResult> GetAllBrands(string sellerId, CancellationToken cancellationToken)
    {
        var query = new GetAllPrandsQuery(sellerId);

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


    [HttpGet("GetAllPaymentMethods/{userId}")]
    public async Task<IActionResult> GetAllPaymentMethods(string userId, CancellationToken cancellationToken)
    {
        var query = new GetAllPaymentMethodsQuery(userId);

        var result = await sender.Send(query, cancellationToken);
        return HandleMediatorResult<List<GetAllPaymentMethodsResponse>>(result);
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
        var command = new CreatProductCommand(request.BrandId,
            request.EnglishName,
            request.ArabicName,
            request.IsFeatured,
            request.IsHidden,
            request.ShippingType,
            request.ProductStatus);

        var result = await sender.Send(command, cancellationToken);
        return HandleMediatorResult<CreateProductResponse>(result);
    }

    [HttpPost("UploadProductImage")]
    public async Task<IActionResult> UploadProductImage(
        [FromForm] UploadProductImageRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UploadProductImageCommand(request.ProductId, request.Image);

        var result = await sender.Send(command, cancellationToken);
        return HandleMediatorResult<UploadProductImageResponse>(result);
    }

    [HttpPost("CreateProductAdditionalInformation")]
    public async Task<IActionResult> CreateProductAdditionalInformation(
        [FromBody] CreateProductAdditionalInformationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddAdditionalInformationToProductCommand(
            request.ProductId,
            request.EnglishDescription,
            request.ArabicDescription,
            request.MinDeleveryDays,
            request.MaxDeleveryDays,
            request.Variants);

        var result = await sender.Send(command, cancellationToken);
        return HandleMediatorResult<CreateProductAdditionalInformationResponse>(result);
    }

    [HttpPost("UploadProductVideo")]
    [RequestSizeLimit(500 * 1024 * 1024)] // 500MB limit
    public async Task<IActionResult> UploadProductVideo([FromForm] UploadVideoRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UploadProductVideoCommand(request.ProductId, request.Video);

        var result = await sender.Send(command, cancellationToken);
        return HandleMediatorResult<UploeadProductVideoResponse>(result);
    }

    [HttpGet("GetAllProductsForSpecificSeller/{sellerId}")]
    public async Task<IActionResult> GetAllProductsForSpecificSeller(string sellerId,
        [FromQuery] PaginatedRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetAllProductsForSpecificSellerQuery(sellerId,request);

        var result = await sender.Send(query, cancellationToken);
        return Ok(result);
    }
}