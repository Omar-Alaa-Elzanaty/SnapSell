using System.Net;
using System.Security.Claims;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Extnesions;
using SnapSell.Domain.Models;


namespace SnapSell.Application.Features.product.Commands.CreateProduct;

internal sealed class CreatProductCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor,
    IValidator<CreatProductCommand> validator)
    : IRequestHandler<CreatProductCommand, Result<CreateProductResponse>>
{
    public async Task<Result<CreateProductResponse>> Handle(CreatProductCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.GetErrorsDictionary();
            return new Result<CreateProductResponse>()
            {
                Errors = errors,
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Validation failed"
            };
        }

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

        var product = new Product
        {
            ArabicName = request.ArabicName,
            EnglishName = request.EnglishName,
            IsFeatured = request.IsFeatured,
            IsHidden = request.IsHidden,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow,
            BrandId = request.BrandId,
            ProductStatus = request.ProductStatus,
            ShippingType = request.ShippingType
        };

        await unitOfWork.ProductsRepo.AddAsync(product);
        await unitOfWork.SaveAsync(cancellationToken);

        var response = new CreateProductResponse(
            ProductId: product.Id,
            SellerId: userId,
            EnglishName: product.EnglishName,
            ArabicName: product.ArabicName,
            IsFeatured: product.IsFeatured,
            IsHidden: product.IsHidden,
            ShippingType: request.ShippingType,
            ProductStatus: request.ProductStatus);

        return Result<CreateProductResponse>.Success(
            data: response,
            message: "Product Created Successfully.",
            statusCode: HttpStatusCode.Created);
    }
}