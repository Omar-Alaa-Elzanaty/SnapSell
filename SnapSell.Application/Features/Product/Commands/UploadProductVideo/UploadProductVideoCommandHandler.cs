using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Extnesions;
using System.Net;
using System.Security.Claims;

namespace SnapSell.Application.Features.product.Commands.UploadProductVideo;

internal sealed class UploadProductVideoCommandHandler(
    IHttpContextAccessor httpContextAccessor,
    IMediaService mediaService,
    IUnitOfWork unitOfWork,
    IValidator<UploadProductVideoCommand> validator)
    : IRequestHandler<UploadProductVideoCommand, Result<UploeadProductVideoResponse>>
{
    public async Task<Result<UploeadProductVideoResponse>> Handle(
        UploadProductVideoCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.GetErrorsDictionary();
            return new Result<UploeadProductVideoResponse>()
            {
                Errors = errors,
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Validation failed"
            };
        }

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

            var product = await unitOfWork.ProductsRepository.GetByIdAsync(request.ProductId);
            if (product is null)
                return Result<UploeadProductVideoResponse>.Failure(
                    $"No product with ID: {request.ProductId}", HttpStatusCode.NotFound);

            if (product.CreatedBy != userId)
                return Result<UploeadProductVideoResponse>.Failure(
                    "No permission to modify this product", HttpStatusCode.Forbidden);

            if (request.Video.Length == 0)
                return Result<UploeadProductVideoResponse>.Failure(
                    "No video file provided", HttpStatusCode.BadRequest);

            // IFormFile ==> MediaFileDto
            using var memoryStream = new MemoryStream();
            await request.Video.CopyToAsync(memoryStream, cancellationToken);
            var fileBytes = memoryStream.ToArray();
            var base64String = Convert.ToBase64String(fileBytes);

            var mediaFileDto = new MediaFileDto
            {
                FileName = request.Video.FileName,
                Base64 = base64String
            };

            var savedFileName = await mediaService.SaveAsync(mediaFileDto, MediaTypes.Video);

            if (string.IsNullOrEmpty(savedFileName))
            {
                return Result<UploeadProductVideoResponse>.Failure(
                    "Video upload failed", HttpStatusCode.BadRequest);
            }

            product.MainVideoUrl = savedFileName;

            await unitOfWork.SaveAsync(cancellationToken);
            var response =
                new UploeadProductVideoResponse(mediaService.GetUrl(savedFileName, MediaTypes.Video));

            return Result<UploeadProductVideoResponse>.Success(
                data: response,
                message: "Product video uploaded successfully",
                statusCode: HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            return Result<UploeadProductVideoResponse>.Failure(
                "An error occurred while uploading the video",
                HttpStatusCode.InternalServerError);
        }
    }
}