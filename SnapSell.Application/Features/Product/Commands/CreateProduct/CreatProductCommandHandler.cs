using Mapster;
using MediatR;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models;
using System.Net;

namespace SnapSell.Application.Features.product.Commands.CreateProduct;

internal sealed class CreatProductCommandHandler(
    IUnitOfWork unitOfWork,
    IMediaService mediaService)
    : IRequestHandler<CreatProductCommand, Result<CreateProductResponse>>
{
    public async Task<Result<CreateProductResponse>> Handle(CreatProductCommand request,
        CancellationToken cancellationToken)
    {
        var imageUrl = await UploadMedia(request.ImageData, MediaTypes.Image);
        var videoUrl = await UploadMedia(request.VideoData, MediaTypes.Video);

        var product = request.Adapt<Product>();
        product.MainImageUrl = imageUrl;
        product.MainVideoUrl = videoUrl;

        await unitOfWork.ProductsRepo.AddAsync(product);
        await AddProductVariants(request.Variants, product.Id);
        await unitOfWork.SaveAsync(cancellationToken);

        var response = product.Adapt<CreateProductResponse>() with
        {
            MainImageUrl = mediaService.GetUrl(product.MainImageUrl, MediaTypes.Image),
            MainVideoUrl = mediaService.GetUrl(product.MainVideoUrl, MediaTypes.Video)
        };

        return Result<CreateProductResponse>.Success(
            data: response,
            message: "Product Created Successfully",
            statusCode: HttpStatusCode.Created);
    }

    private async Task<string?> UploadMedia(byte[] image, MediaTypes mediaTypes)
    {
        var fileName = await mediaService.SaveAsync(new MediaFileDto
        {
            FileName = mediaTypes == MediaTypes.Image ? $"{Guid.NewGuid()}.png" : $"{Guid.NewGuid()}.mp4",
            Base64 = Convert.ToBase64String(image)
        }, MediaTypes.Image);

        return fileName;
    }

    private async Task AddProductVariants(List<CreatProductVariantDto?> variants, Guid productId)
    {
        if (variants?.Count > 0)
        {
            var productVariants = variants
                .Where(v => v is not null)
                .Select(v => new Variant()
                {
                    ProductId = productId,
                    Size = v!.Size,
                    Color = v.Color,
                    Quantity = v.Quantity,
                    Price = v.Price,
                    RegularPrice = v.RegularPrice,
                    SalePrice = v.SalePrice,
                    SKU = v.SKU,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

            await unitOfWork.VariantsRepo.AddRange(productVariants);
        }
    }
}