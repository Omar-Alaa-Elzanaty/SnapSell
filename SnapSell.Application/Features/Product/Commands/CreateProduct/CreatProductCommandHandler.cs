using System.Net;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.DTOs.Product;
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

        string? imageUrl = null;
        if (request.MainImageUrl.Length > 0)
        {
            using var memoryStream = new MemoryStream();
            await request.MainImageUrl.CopyToAsync(memoryStream, cancellationToken);
            var base64 = Convert.ToBase64String(memoryStream.ToArray());

            var mediaDto = new MediaFileDto
            {
                FileName = request.MainImageUrl.FileName,
                Base64 = base64
            };

            var savedPath = await mediaService.SaveAsync(mediaDto, MediaTypes.Image);
            imageUrl = mediaService.GetUrl(savedPath);
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
            MainImageUrl = imageUrl
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
            MainImageUrl: product.MainImageUrl
        );

        return Result<CreateProductResponse>.Success(
            data: response,
            message: "Product Created Successfully.",
            statusCode: HttpStatusCode.Created);
    }
}