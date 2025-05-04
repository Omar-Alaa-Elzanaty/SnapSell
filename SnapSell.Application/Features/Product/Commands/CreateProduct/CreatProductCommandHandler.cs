using System.Net;
using System.Security.Claims;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.DTOs.Product;
using SnapSell.Application.DTOs.variant;
using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models;


namespace SnapSell.Application.Features.product.Commands.CreateProduct;

internal sealed class CreatProductCommandHandler(
    ISQLBaseRepo<Product> productRepository,
    ISQLBaseRepo<Variant> variantRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor,
    IMediaService mediaService)
    : IRequestHandler<CreatProductCommand, Result<CreateProductResponse>>
{
    public async Task<Result<CreateProductResponse>> Handle(CreatProductCommand request,
        CancellationToken cancellationToken)
    {
        var currentUser = httpContextAccessor.HttpContext?.User;
        if (currentUser is null)
        {
            return Result<CreateProductResponse>.Failure(
                message: "Current user is null",
                HttpStatusCode.Unauthorized);
        }

        var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<CreateProductResponse>.Failure(
                message: "User ID not found in claims",
                HttpStatusCode.Unauthorized);
        }

        var imageUrl = await ProcessMediaUpload(request.MainImageUrl, MediaTypes.Image);
        var videoUrl = await ProcessMediaUpload(request.MainVideoUrl, MediaTypes.Video);

        var product = new Product
        {
            ArabicName = request.ArabicName,
            EnglishName = request.EnglishName,
            Description = request.Description,
            ShortDescription = request.ShortDescription,
            IsFeatured = request.IsFeatured,
            IsHidden = request.IsHidden,
            MinDeleveryDays = request.MinDeleveryDays,
            MaxDeleveryDays = request.MaxDeleveryDays,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow,
            MainVideoUrl = videoUrl,
            MainImageUrl = imageUrl,
            Variants = request.Variants?.Select(v => new Variant
            {
                SizeId = v.SizeId,
                ColorId = v.ColorId,
                Quantity = v.Quantity,
                Price = v.Price,
                RegularPrice = v.RegularPrice,
                SalePrice = v.SalePrice,
                SKU = v.SKU ?? GenerateSku(),
                Barcode = v.Barcode ?? GenerateBarcode()
            }).ToList() ?? new List<Variant>()
        };

        await productRepository.AddAsync(product);
        foreach (var variant in product.Variants)
        {
            variant.ProductId = product.Id;
            await variantRepository.AddAsync(variant);
        }

        await unitOfWork.SaveAsync(cancellationToken);

        var response = new CreateProductResponse(
            ProductId: product.Id,
            SellerId: userId,
            EnglishName: product.EnglishName,
            ArabicName: product.ArabicName,
            Description: product.Description,
            ShortDescription: product.ShortDescription,
            IsFeatured: product.IsFeatured,
            IsHidden: product.IsHidden,
            MinDeleveryDays: product.MinDeleveryDays,
            MaxDeleveryDays: product.MaxDeleveryDays,
            MainImageUrl: product.MainImageUrl,
            MainVideoUrl: product.MainVideoUrl,
            Variants: product.Variants?.Select(v => v.Adapt<VariantResponse>()).ToList() ?? []
        );

        return Result<CreateProductResponse>.Success(
            data: response,
            message: "Product Created Successfully.",
            statusCode: HttpStatusCode.Created);
    }

    private string GenerateSku()
    {
        return $"SKU-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    private string GenerateBarcode()
    {
        return $"{DateTime.Now:yyyyMMdd}-{new Random().Next(100000, 999999)}";
    }

    private async Task<string?> ProcessMediaUpload(IFormFile file, MediaTypes mediaType)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var base64 = Convert.ToBase64String(memoryStream.ToArray());

            var fileExtension = Path.GetExtension(file.FileName);
            var newFileName = $"{Guid.NewGuid()}{fileExtension}";

            var mediaDto = new MediaFileDto
            {
                FileName = newFileName,
                Base64 = base64
            };

            var savedPath = await mediaService.SaveAsync(mediaDto, mediaType);
            return mediaService.GetUrl(savedPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing media upload: {ex.Message}");
            return null;
        }
    }
}