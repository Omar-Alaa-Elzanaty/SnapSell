using System.Net;
using System.Security.Claims;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnapSell.Application.DTOs.media;
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
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor,
    IMediaService mediaService,
    IVideoUploadService videoUploadService,
    ILogger<CreatProductCommandHandler> logger,
    IConfiguration configuration)
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

        var imageUrl = await ProcessImageUpload(request.MainImageUrl, MediaTypes.Image);
        if (imageUrl is null)
        {
            return Result<CreateProductResponse>.Failure(
                message: "Failed to upload product image",
                HttpStatusCode.BadRequest);
        }

        var videoResult = await UploadVideoAsync(request.Video);

        // Check for failure conditions
        if (videoResult.FailedUploads.Any() || !videoResult.IsComplete || string.IsNullOrEmpty(videoResult.PublicUrl))
        {
            var error = videoResult.FailedUploads.FirstOrDefault();
            var errorMessage = error != default
                ? $"Video upload failed: {error.error}"
                : "Video processing failed (no valid URL generated)";

            logger.LogError("Video upload failed: {ErrorDetails}", errorMessage);

            return Result<CreateProductResponse>.Failure(
                message: errorMessage,
                HttpStatusCode.BadRequest);
        }

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
            MainVideoUrl = videoResult.RelativePath,
            MainImageUrl = imageUrl,
            BrandId = request.BrandId
            //Variants = request.Variants?.Select(v => new Variant
            //{
            //    SizeId = v.SizeId,
            //    ColorId = v.ColorId,
            //    Quantity = v.Quantity,
            //    Price = v.Price,
            //    RegularPrice = v.RegularPrice,
            //    SalePrice = v.SalePrice,
            //    SKU = v.SKU ?? GenerateSku(),
            //    Barcode = v.Barcode ?? GenerateBarcode()
            //}).ToList() ?? new List<Variant>()
        };

        await productRepository.AddAsync(product);
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
            MainImageUrl: mediaService.GetUrl(product.MainImageUrl),
            MainVideoUrl: videoUploadService.GetFullUrl(videoResult.RelativePath),
            Variants: product.Variants?.Select(v => v.Adapt<VariantResponse>()).ToList() ?? []
        );

        return Result<CreateProductResponse>.Success(
            data: response,
            message: "Product Created Successfully.",
            statusCode: HttpStatusCode.Created);
    }

    private async Task<string?> ProcessImageUpload(IFormFile file, MediaTypes mediaType)
    {
        if (file.Length == 0 ) return null;

        try
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var mediaDto = new MediaFileDto
            {
                FileName = file.FileName,
                Base64 = Convert.ToBase64String(memoryStream.ToArray())
            };

            return await mediaService.SaveAsync(mediaDto, mediaType);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing image upload");
            return null;
        }
    }

    private async Task<VideoUploadResult> UploadVideoAsync(IFormFile videoFile)
    {
        if (videoFile.Length == 0)
        {
            var result = new VideoUploadResult();
            result.FailedUploads.Add(("null", "No video file provided"));
            return result;
        }

        try
        {
            var chunkDto = new VideoChunkDto
            {
                UploadId = Guid.NewGuid().ToString(),
                FileName = videoFile.FileName,
                ChunkNumber = 1,
                TotalChunks = 1,
                TotalFileSize = videoFile.Length,
                Chunk = videoFile
            };

            var result = await videoUploadService.UploadChunkAsync(chunkDto);
            result.EndTime = DateTime.UtcNow;

            // Get the relative path from the upload service
            if (!string.IsNullOrEmpty(result.FilePath))
            {
                var relativeFolder = configuration["MediaSavePaths:Videos"] ?? "videos";
                var fileName = Path.GetFileName(result.FilePath);
                result.RelativePath = $"{relativeFolder}/{fileName}".Replace("\\", "/");

                // Ensure PublicUrl is set correctly
                result.PublicUrl = videoUploadService.GetFullUrl(result.RelativePath);
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Video upload failed for {FileName}", videoFile.FileName);
            var result = new VideoUploadResult();
            result.FailedUploads.Add((videoFile.FileName, ex.Message));
            return result;
        }
    }

    private string GenerateSku()
    {
        return $"SKU-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    private string GenerateBarcode()
    {
        return $"{DateTime.Now:yyyyMMdd}-{new Random().Next(100000, 999999)}";
    }
}