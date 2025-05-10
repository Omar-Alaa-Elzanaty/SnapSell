using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.DTOs.media;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Net;
using System.Security.Claims;
using SnapSell.Application.DTOs.Product;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models;
using Microsoft.Extensions.Logging;

namespace SnapSell.Application.Features.product.Commands.UploadProductVideo;

internal sealed class UploadProductVideoCommandHandler(
    ISQLBaseRepo<Product> productRepository,
    IHttpContextAccessor httpContextAccessor,
    IMediaService mediaService,
    IUnitOfWork unitOfWork,
    ILogger<UploadProductVideoCommandHandler> logger)
    : IRequestHandler<UploadProductVideoCommand, Result<UploeadProductVideoResponse>>
{
    public async Task<Result<UploeadProductVideoResponse>> Handle(
        UploadProductVideoCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var currentUser = httpContextAccessor.HttpContext?.User;
            if (currentUser is null)
                return Result<UploeadProductVideoResponse>.Failure(
                    "Current user is null", HttpStatusCode.Unauthorized);

            var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Result<UploeadProductVideoResponse>.Failure(
                    "User ID not found", HttpStatusCode.Unauthorized);

            var product = await productRepository.GetByIdAsync(request.ProductId);
            if (product is null)
                return Result<UploeadProductVideoResponse>.Failure(
                    $"No product with ID: {request.ProductId}", HttpStatusCode.NotFound);

            if (product.CreatedBy != userId)
                return Result<UploeadProductVideoResponse>.Failure(
                    "No permission to modify this product", HttpStatusCode.Forbidden);

            if (request.Video.Length == 0)
                return Result<UploeadProductVideoResponse>.Failure(
                    "No video file provided", HttpStatusCode.BadRequest);

            var chunkDto = new VideoChunkDto
            {
                UploadId = Guid.NewGuid().ToString(),
                FileName = $"{product.Id}_{request.Video.FileName}",
                ChunkNumber = 1,
                TotalChunks = 1,
                TotalFileSize = request.Video.Length,
                Chunk = request.Video
            };

            var uploadResult = await mediaService.UploadChunkAsync(chunkDto);

            if (!uploadResult.IsComplete || string.IsNullOrEmpty(uploadResult.PublicUrl))
            {
                var error = uploadResult.FailedUploads.FirstOrDefault();
                return Result<UploeadProductVideoResponse>.Failure(
                    error != default ? error.error : "Video processing failed",
                    HttpStatusCode.BadRequest);
            }

            product.MainVideoUrl = uploadResult.SavedFileName;
            product.LastUpdatedAt = DateTime.UtcNow;
            product.LastUpdatedBy = userId;

            await unitOfWork.SaveAsync(cancellationToken);
            var response =
                new UploeadProductVideoResponse(mediaService.GetUrl(uploadResult.SavedFileName, MediaTypes.Video));

            return Result<UploeadProductVideoResponse>.Success(
                data: response,
                "Product video uploaded successfully",
                HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading product video for product {ProductId}", request.ProductId);
            return Result<UploeadProductVideoResponse>.Failure(
                "An error occurred while uploading the video",
                HttpStatusCode.InternalServerError);
        }
    }
}