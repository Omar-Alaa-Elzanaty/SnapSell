using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnapSell.Application.DTOs.media;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Net;
using System.Security.Claims;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models;

namespace SnapSell.Application.Features.product.Commands.UploadProductVideo;

internal sealed class UploadProductVideoCommandHandler(
    ISQLBaseRepo<Product> productRepository,
    IHttpContextAccessor httpContextAccessor,
    IVideoUploadService videoUploadService,
    IConfiguration configuration,
    IUnitOfWork unitOfWork,
    ILogger<UploadProductVideoCommandHandler> logger)
    : IRequestHandler<UploadProductVideoCommand, Result<UploeadProductVideoResponse>>
{
    public async Task<Result<UploeadProductVideoResponse>> Handle(UploadProductVideoCommand request,
        CancellationToken cancellationToken)
    {
        var currentUser = httpContextAccessor.HttpContext?.User;
        if (currentUser is null)
        {
            return Result<UploeadProductVideoResponse>.Failure(
                message: "Current user is null",
                HttpStatusCode.Unauthorized);
        }

        var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<UploeadProductVideoResponse>.Failure(
                message: "User ID not found in claims",
                HttpStatusCode.Unauthorized);
        }

        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
        {
            return Result<UploeadProductVideoResponse>.Failure(
                message: $"No product with this Id :{request.ProductId}.",
                HttpStatusCode.BadRequest);
        }

        if (product.CreatedBy != userId)
        {
            return Result<UploeadProductVideoResponse>.Failure(
                message: "You don't have permission to modify this product",
                HttpStatusCode.Forbidden);
        }

        if (request.Video.Length == 0)
        {
            return Result<UploeadProductVideoResponse>.Failure(
                message: "No video file provided",
                HttpStatusCode.BadRequest);
        }

        try
        {
            // Prepare chunk DTO (single chunk upload)
            var chunkDto = new VideoChunkDto
            {
                UploadId = Guid.NewGuid().ToString(),
                FileName = $"{product.Id}_{request.Video.FileName}",
                ChunkNumber = 1,
                TotalChunks = 1,
                TotalFileSize = request.Video.Length,
                Chunk = request.Video
            };

            // Process upload
            var uploadResult = await videoUploadService.UploadChunkAsync(chunkDto);

            // Handle failures
            if (uploadResult.FailedUploads.Any() || !uploadResult.IsComplete || string.IsNullOrEmpty(uploadResult.PublicUrl))
            {
                var error = uploadResult.FailedUploads.FirstOrDefault();
                var errorMessage = error != default
                    ? $"Video upload failed: {error.error}"
                    : "Video processing failed";

                logger.LogError("Video upload failed for product {ProductId}: {Error}", request.ProductId, errorMessage);
                return Result<UploeadProductVideoResponse>.Failure(
                    message: errorMessage,
                    HttpStatusCode.BadRequest);
            }

            // Extract relative path (remove base URL)
            var baseUrl = configuration["MediaBaseUrl"]?.TrimEnd('/') ?? "";
            var relativePath = uploadResult.PublicUrl.Replace(baseUrl, "").TrimStart('/');

            product.MainVideoUrl = relativePath;
            product.LastUpdatedAt = DateTime.UtcNow;
            product.LastUpdatedBy = userId;

            await unitOfWork.SaveAsync(cancellationToken);

            return Result<UploeadProductVideoResponse>.Success(
                data: new UploeadProductVideoResponse(uploadResult.PublicUrl),
                message: "Product video uploaded successfully",
                statusCode: HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Video upload failed for product {ProductId}", request.ProductId);
            return Result<UploeadProductVideoResponse>.Failure(
                message: "An error occurred while uploading the video",
                HttpStatusCode.InternalServerError);
        }
    }
}