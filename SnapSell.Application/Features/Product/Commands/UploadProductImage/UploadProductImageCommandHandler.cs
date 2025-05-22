using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Net;
using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Extnesions;

namespace SnapSell.Application.Features.product.Commands.UploadProductImage;

internal sealed class UploadProductImageCommandHandler(
    IMediaService mediaService,
    IHttpContextAccessor httpContextAccessor,
    ISQLBaseRepo<Product> productRepository,
    IUnitOfWork unitOfWork,
    IValidator<UploadProductImageCommand> validator)
    : IRequestHandler<UploadProductImageCommand, Result<UploadProductImageResponse>>
{
    public async Task<Result<UploadProductImageResponse>> Handle(UploadProductImageCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.GetErrorsDictionary();
            return new Result<UploadProductImageResponse>()
            {
                Errors = errors,
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Validation failed"
            };
        }
        var currentUser = httpContextAccessor.HttpContext?.User;
        if (currentUser is null)
        {
            return Result<UploadProductImageResponse>.Failure(
                message: "Current user is null",
                HttpStatusCode.Unauthorized);
        }

        var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<UploadProductImageResponse>.Failure(
                message: "User ID not found in claims",
                HttpStatusCode.Unauthorized);
        }

        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
        {
            return Result<UploadProductImageResponse>.Failure(
                message: $"No product found with this Id : {request.ProductId}",
                HttpStatusCode.NotFound);
        }

        if (product.CreatedBy != userId)
        {
            return Result<UploadProductImageResponse>.Failure(
                message: "You don't have permission to update this product",
                HttpStatusCode.Forbidden);
        }

        var imageUrl = await ProcessImageUpload(request.Image, MediaTypes.Image);
        if (string.IsNullOrEmpty(imageUrl))
        {
            return Result<UploadProductImageResponse>.Failure(
                message: "Failed to upload product image",
                HttpStatusCode.InternalServerError);
        }

        product.MainImageUrl = imageUrl;
        productRepository.UpdateAsync(product);
        await unitOfWork.SaveAsync(cancellationToken);

        var response = new UploadProductImageResponse(mediaService.GetUrl(imageUrl, MediaTypes.Image));
        return Result<UploadProductImageResponse>.Success(
            data: response,
            message: "Product image uploaded successfully",
            statusCode: HttpStatusCode.OK);
    }

    private async Task<string?> ProcessImageUpload(IFormFile file, MediaTypes mediaType)
    {
        if (file == null || file.Length == 0) return null;

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
            return null;
        }
    }
}