using Mapster;
using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.products.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler(
    IUnitOfWork unitOfWork,
    IMediaService mediaService,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<CreateProductCommand, Result<CreateProductResponse>>
{
    public async Task<Result<CreateProductResponse>> Handle(CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
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
                message: "Categories not found",
                statusCode: HttpStatusCode.BadRequest);
        }

        var store = await unitOfWork.StoresRepo.Entities.Where(x => x.AccountId == userId)
            .SingleOrDefaultAsync(cancellationToken);

        if (store is null)
        {
            return Result<CreateProductResponse>.Failure(
                message: "Current user has no store.",
                statusCode: HttpStatusCode.BadRequest);
        }
        
        var product = request.Adapt<Product>();
        product.BrandId = request.BrandId;
        product.StoreId = store.Id;
        product.Images = new List<ProductImage>();

        await unitOfWork.ProductsRepo.AddAsync(product);
        await unitOfWork.SaveAsync(cancellationToken);

        foreach (var image in request.Images)
        {
            var imageUrl = await mediaService.SaveAsync(image, MediaTypes.Image);
            if (string.IsNullOrEmpty(imageUrl))
            {
                return Result<CreateProductResponse>.Failure(
                    message: "Failed to save product image",
                    statusCode: HttpStatusCode.InternalServerError);
            }

            product.Images.Add(new ProductImage
            {
                ImageUrl = imageUrl,
                IsMainImage = image.IsMain,
                ProductId = product.Id
            });
        }

        await unitOfWork.ProductImagesRepo.AddRange(product.Images);
        await unitOfWork.SaveAsync(cancellationToken);

        if (request.HasVariants)
        {
            var sizes = await unitOfWork.SizesRepo.Entities
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            var variants = new List<Variant>();
            foreach (var variantDto in request.Variants!)
            {
                if (!sizes.Contains(variantDto.SizeId))
                {
                    return Result<CreateProductResponse>.Failure(
                        message: $"Invalid size ID: {variantDto.SizeId}",
                        statusCode: HttpStatusCode.BadRequest);
                }

                var variant = variantDto.Adapt<Variant>();
                variant.Id = Guid.NewGuid();
                variant.ProductId = product.Id;
                variants.Add(variant);
            }

            await unitOfWork.VariantsRepo.AddRange(variants);
        }
        else
        {
            product.Price = request.Price;
            product.SalePrice = request.SalePrice;
            product.CostPrice = request.CostPrice;
            product.Quantity = request.Quantity;
            product.Sku = request.Sku;

            unitOfWork.ProductsRepo.Update(product);
        }

        var categoryParentMappings = await unitOfWork.CategoryRepo.Entities
            .Where(c => request.CategoryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, c => c.ParentCategoryId, cancellationToken);
        
        var productCategories = request.CategoryIds.Select(categoryId => new ProductCategory
        {
            ProductId = product.Id,
            CategoryId = categoryId,
            ParentCategoryId = categoryParentMappings.TryGetValue(categoryId, out var parentId) 
                ? parentId : null
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