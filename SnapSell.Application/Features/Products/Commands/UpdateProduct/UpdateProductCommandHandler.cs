using System.Net;
using System.Security.Claims;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler(
    IUnitOfWork unitOfWork,
    IMediaService mediaService,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<UpdateProductCommand, Result<UpdateProductResponse>>
{
    public async Task<Result<UpdateProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Result<UpdateProductResponse>.Failure("Unauthorized", HttpStatusCode.Unauthorized);
        }

        var product = await unitOfWork.ProductsRepo.Entities
            .Include(p => p.Images)
            .Include(p => p.Variants)
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            return Result<UpdateProductResponse>.Failure(
                $"Product with ID {request.ProductId} does not exist"
                , HttpStatusCode.NotFound);
        }

        var store = await unitOfWork.StoresRepo.Entities
            .FirstOrDefaultAsync(x => x.SellerId == userId, cancellationToken);

        if (store == null || product.StoreId != store.Id)
        {
            return Result<UpdateProductResponse>.Failure("You don't have permission to update this product", HttpStatusCode.Forbidden);
        }

        var brandExists = await unitOfWork.BrandsRepo.Entities
            .AnyAsync(b => b.Id == request.BrandId, cancellationToken);

        if (!brandExists)
        {
            return Result<UpdateProductResponse>.Failure($"Brand with ID {request.BrandId} does not exist", HttpStatusCode.BadRequest);
        }

        var existingCategoryIds = await unitOfWork.CategoryRepo.Entities
            .Where(c => request.CategoryIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync(cancellationToken);

        var missingCategoryIds = request.CategoryIds.Except(existingCategoryIds).ToList();
        if (missingCategoryIds.Any())
        {
            return Result<UpdateProductResponse>.Failure("Some categories not found", HttpStatusCode.BadRequest);
        }

        product.BrandId = request.BrandId;
        product.EnglishName = request.EnglishName;
        product.ArabicName = request.ArabicName;
        product.IsFeatured = request.IsFeatured;
        product.IsHidden = request.IsHidden;
        product.ProductStatus = request.ProductStatus;
        product.HasVariants = request.HasVariants;
        product.ShippingType = (ShippingType)request.ShippingType;
        product.ProductType = request.ProductType;
        product.EnglishDescription = request.EnglishDescription;
        product.ArabicDescription = request.ArabicDescription;
        product.MinDeliveryDays = request.MinDeliveryDays;
        product.MaxDeliveryDays = request.MaxDeliveryDays;

        if (request.Images?.Any() == true)
        {
            var newImages = new List<ProductImage>();
            foreach (var imageDto in request.Images)
            {
                var imageUrl = await mediaService.SaveAsync(imageDto, MediaTypes.Image);
                if (string.IsNullOrEmpty(imageUrl))
                {
                    return Result<UpdateProductResponse>.Failure("Failed to save product image", HttpStatusCode.InternalServerError);
                }

                newImages.Add(new ProductImage
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    ImageUrl = imageUrl,
                    IsMainImage = imageDto.IsMain
                });
            }

            unitOfWork.ProductImagesRepo.RemoveRange(product.Images);
            await unitOfWork.ProductImagesRepo.AddRange(newImages);
            product.Images = newImages;
        }

        if (request.HasVariants)
        {
            var validSizeIds = await unitOfWork.SizesRepo.Entities
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            var variantsToUpdate = new List<Variant>();
            var variantIdsToKeep = new List<Guid>();

            foreach (var variantDto in request.Variants!)
            {
                if (!validSizeIds.Contains(variantDto.SizeId))
                {
                    return Result<UpdateProductResponse>.Failure($"Invalid size ID: {variantDto.SizeId}", HttpStatusCode.BadRequest);
                }

                if (variantDto.Id != Guid.Empty)
                {
                    var existingVariant = product.Variants.FirstOrDefault(v => v.Id == variantDto.Id);
                    if (existingVariant != null)
                    {
                        variantDto.Adapt(existingVariant);
                        variantsToUpdate.Add(existingVariant);
                        variantIdsToKeep.Add(existingVariant.Id);
                        continue;
                    }
                }

                var newVariant = variantDto.Adapt<Variant>();
                newVariant.Id = Guid.NewGuid();
                newVariant.ProductId = product.Id;
                variantsToUpdate.Add(newVariant);
                variantIdsToKeep.Add(newVariant.Id);
            }

            var variantsToRemove = product.Variants
                .Where(v => !variantIdsToKeep.Contains(v.Id))
                .ToList();

            unitOfWork.VariantsRepo.RemoveRange(variantsToRemove);
            unitOfWork.VariantsRepo.UpdateRange(variantsToUpdate);
            product.Variants = variantsToUpdate;
        }
        else
        {
            if (product.Variants.Any())
            {
                unitOfWork.VariantsRepo.RemoveRange(product.Variants);
                product.Variants.Clear();
            }

            product.Price = request.Price;
            product.SalePrice = request.SalePrice;
            product.CostPrice = request.CostPrice;
            product.Quantity = request.Quantity;
            product.Sku = request.Sku;
        }

        var currentCategoryIds = product.Categories.Select(pc => pc.CategoryId).ToList();
        var categoriesToAdd = request.CategoryIds.Except(currentCategoryIds).ToList();
        var categoriesToRemove = currentCategoryIds.Except(request.CategoryIds).ToList();

        if (categoriesToRemove.Any())
        {
            var productCategoriesToRemove = product.Categories
                .Where(pc => categoriesToRemove.Contains(pc.CategoryId))
                .ToList();

            unitOfWork.ProductCategoriesRepo.RemoveRange(productCategoriesToRemove);
        }

        if (categoriesToAdd.Any())
        {
            var categoryParentMappings = await unitOfWork.CategoryRepo.Entities
                .Where(c => categoriesToAdd.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, c => c.ParentCategoryId, cancellationToken);

            var newProductCategories = categoriesToAdd.Select(categoryId => new ProductCategory
            {
                ProductId = product.Id,
                CategoryId = categoryId,
                ParentCategoryId = categoryParentMappings.TryGetValue(categoryId, out var parentId) ? parentId : null
            }).ToList();

            await unitOfWork.ProductCategoriesRepo.AddRange(newProductCategories);
        }

        unitOfWork.ProductsRepo.Update(product);
        await unitOfWork.SaveAsync(cancellationToken);

        var response = product.Adapt<UpdateProductResponse>();
        foreach (var image in response.Images)
        {
            image.ImageUrl = mediaService.GetUrl(image.ImageUrl, MediaTypes.Image)!;
        }

        return Result<UpdateProductResponse>.Success(response, "Product Updated Successfully", HttpStatusCode.OK);
    }
}