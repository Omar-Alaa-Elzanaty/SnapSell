using System.Net;
using System.Security.Claims;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Extnesions;
using SnapSell.Domain.Models;

namespace SnapSell.Application.Features.product.Commands.AddAdditionalInformationToProduct;

internal sealed class AddAdditionalInformationToProductCommandHandler(
    IHttpContextAccessor httpContextAccessor,
    ISQLBaseRepo<Product> productRepository,
    ISQLBaseRepo<Variant> variantsRepository,
    IUnitOfWork unitOfWork,
    IValidator<AddAdditionalInformationToProductCommand> validator)
    : IRequestHandler<AddAdditionalInformationToProductCommand,
        Result<CreateProductAdditionalInformationResponse>>
{
    public async Task<Result<CreateProductAdditionalInformationResponse>> Handle(
        AddAdditionalInformationToProductCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.GetErrorsDictionary();
            return new Result<CreateProductAdditionalInformationResponse>()
            {
                Errors = errors,
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Validation failed"
            };
        }

        var currentUser = httpContextAccessor.HttpContext?.User;
        if (currentUser is null)
        {
            return Result<CreateProductAdditionalInformationResponse>.Failure(
                message: "Current user is null",
                HttpStatusCode.Unauthorized);
        }

        var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<CreateProductAdditionalInformationResponse>.Failure(
                message: "User ID not found in claims",
                HttpStatusCode.Unauthorized);
        }


        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
        {
            return Result<CreateProductAdditionalInformationResponse>.Failure(
                message: $"No product found with this Id : {request.ProductId}",
                HttpStatusCode.NotFound);
        }

        product.ArabicDescription = request.EnglishDescription;
        product.ArabicDescription = request.ArabicDescription;
        product.MinDeleveryDays = request.MinDeleveryDays;
        product.MaxDeleveryDays = request.MaxDeleveryDays;
        product.LastUpdatedBy = userId;
        product.LastUpdatedAt = DateTime.UtcNow;

        var variantEntities = new List<Variant>();
        foreach (var variantDto in request.Variants)
        {
            if (variantDto is null) continue;

            var variant = new Variant
            {
                ProductId = request.ProductId,
                SizeId = variantDto.SizeId,
                ColorId = variantDto.ColorId,
                Quantity = variantDto.Quantity,
                Price = variantDto.Price,
                RegularPrice = variantDto.RegularPrice,
                SalePrice = variantDto.SalePrice,
                SKU = variantDto.SKU ?? GenerateSku(),
                Barcode = variantDto.Barcode ?? GenerateBarcode(),
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,
            };

            variantEntities.Add(variant);
        }

        try
        {
            productRepository.UpdateAsync(product);
            await variantsRepository.AddRange(variantEntities);
            await unitOfWork.SaveAsync(cancellationToken);

            var response = new CreateProductAdditionalInformationResponse(
                ProductId: product.Id,
                SellerId: userId!,
                EnglishName: product.EnglishName,
                ArabicName: product.ArabicName,
                EnglishDescription: product.EnglishDescription!,
                ArabicDescription: product.ArabicDescription,
                IsFeatured: product.IsFeatured,
                IsHidden: product.IsHidden,
                MinDeleveryDays: product.MinDeleveryDays,
                MaxDeleveryDays: product.MaxDeleveryDays,
                MainImageUrl: product.MainImageUrl,
                Variants: variantEntities.Adapt<List<VariantResponse>>() ?? []
            );

            return Result<CreateProductAdditionalInformationResponse>.Success(
                data: response,
                message: "Product additional information added successfully",
                statusCode: HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return Result<CreateProductAdditionalInformationResponse>.Failure(
                message: "An error occurred while saving product information.",
                HttpStatusCode.InternalServerError);
        }
    }

    private string GenerateSku() => $"SKU-{Guid.NewGuid().ToString()[..8].ToUpper()}";

    private string GenerateBarcode() => $"{DateTime.Now:yyyyMMdd}-{new Random().Next(100000, 999999)}";
}