using Mapster;
using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using System.Net;
using Microsoft.EntityFrameworkCore;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.products.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler(
    IUnitOfWork unitOfWork,
    IMediaService mediaService) : IRequestHandler<CreateProductCommand, Result<CreateProductResponse>>
{
    public async Task<Result<CreateProductResponse>> Handle(CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var brandExists = await unitOfWork.BrandsRepo.Entities
            .AnyAsync(b => b.Id == request.BrandId, cancellationToken);
        
        if (!brandExists)
        {
            return Result<CreateProductResponse>.Failure(
                message: $"Brand with ID {request.BrandId} does not exist",
                statusCode: HttpStatusCode.BadRequest);
        }

        var existingCategoryIds = await unitOfWork.CategoryRepo.Entities
            .Where(c => request.CategoryIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync(cancellationToken);

        var missingCategoryIds = request.CategoryIds.Except(existingCategoryIds).ToList();
        if (missingCategoryIds.Any())
        {
            return Result<CreateProductResponse>.Failure(
                message: "Categories not found.",
                statusCode: HttpStatusCode.BadRequest);
        }
        var product = request.Adapt<Product>();
        product.BrandId = request.BrandId;
        
        product.Images = new List<ProductImage>();
        foreach (var image in request.Images)
        {
            product.Images.Add(new ProductImage
            {
                ImageUrl = await mediaService.SaveAsync(image, MediaTypes.Image) ?? "",
                IsMainImage = image.IsMain
            });
        }

        var sizes = await unitOfWork.SizesRepo.Entities.Select(x => x.Id).ToListAsync(cancellationToken);
        if (request.HasVariants)
        {
            product.Variants = request.Variants.Adapt<List<Variant>>();
            foreach (var variant in product.Variants)
            {
                if (!sizes.Contains(variant.SizeId))
                {
                    return Result<CreateProductResponse>.Failure(
                        message: $"Invalid size ID: {variant.SizeId}",
                        statusCode: HttpStatusCode.BadRequest);
                }

                variant.Id = Guid.NewGuid();
                variant.ProductId = product.Id;
            }
        }
        else
        {
            product.Price = request.Price;
            product.SalePrice = request.SalePrice;
            product.CostPrice = request.CostPrice;
            product.Quantity = request.Quantity;
            product.Sku = request.Sku;
        }
        
        await unitOfWork.ProductsRepo.AddAsync(product);
        var productCategories = request.CategoryIds.Select(categoryId => new ProductCategory
        {
            ProductId = product.Id,
            CategoryId = categoryId
        }).ToList();

        await unitOfWork.ProductCategoriesRepo.AddRange(productCategories);
        await unitOfWork.SaveAsync(cancellationToken);

        var response = product.Adapt<CreateProductResponse>();
        foreach (var image in response.Images)
        {
            image.ImageUrl = mediaService.GetUrl(image.ImageUrl, MediaTypes.Image)!;
        }

        return Result<CreateProductResponse>.Success(
            data: response,
            message: "Product Created Successfully",
            statusCode: HttpStatusCode.Created);
    }
}