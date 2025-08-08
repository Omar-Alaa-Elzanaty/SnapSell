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

namespace SnapSell.Application.Features.Products.Commands.UpdateProductBasicInfo;

internal sealed class UpdateProductBasicInfoCommandHandler(
    IUnitOfWork unitOfWork,
    IMediaService mediaService,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<UpdateProductBasicInfoCommand, Result<UpdateProductBasicInfoResponse>>
{
    public async Task<Result<UpdateProductBasicInfoResponse>> Handle(UpdateProductBasicInfoCommand request,
        CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Result<UpdateProductBasicInfoResponse>.Failure(
                "Unauthorized", 
                HttpStatusCode.Unauthorized);
        }

        var product = await unitOfWork.ProductsRepo.Entities
            .Include(p => p.Images)
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            return Result<UpdateProductBasicInfoResponse>.Failure(
                message:$"Product with ID {request.ProductId} not found",
                statusCode:HttpStatusCode.NotFound);
        }

        var store = await unitOfWork.StoresRepo.Entities
            .SingleOrDefaultAsync(s => s.AccountId == userId, cancellationToken);

        if (store == null || product.StoreId != store.Id)
        {
            return Result<UpdateProductBasicInfoResponse>.Failure(
                message:"Unauthorized access to this product",
                statusCode:HttpStatusCode.Forbidden);
        }

        var brandExists = await unitOfWork.BrandsRepo.Entities
            .AnyAsync(b => b.Id == request.BrandId, cancellationToken);
        if (!brandExists)
        {
            return Result<UpdateProductBasicInfoResponse>.Failure(
                message:"Brand does not exist",
                statusCode:HttpStatusCode.NotFound);
        }

        var validCategoryIds = await unitOfWork.CategoryRepo.Entities
            .Where(c => request.CategoryIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync(cancellationToken);

        var missingCategoryIds = request.CategoryIds.Except(validCategoryIds).ToList();
        if (missingCategoryIds.Any())
        {
            return Result<UpdateProductBasicInfoResponse>.Failure(
                message:"Some categories not found",
                statusCode:HttpStatusCode.NotFound);
        }

        product.BrandId = request.BrandId;
        product.EnglishName = request.EnglishName;
        product.ArabicName = request.ArabicName;
        product.IsFeatured = request.IsFeatured;
        product.IsHidden = request.IsHidden;
        product.ProductStatus = request.ProductStatus;
        product.ShippingType = (ShippingType)request.ShippingType;
        product.ProductType = request.ProductType;
        product.EnglishDescription = request.EnglishDescription;
        product.ArabicDescription = request.ArabicDescription;
        product.MinDeliveryDays = request.MinDeliveryDays;
        product.MaxDeliveryDays = request.MaxDeliveryDays;

        if (request.Images.Any())
        {
            var newImages = new List<ProductImage>();
            foreach (var imageDto in request.Images)
            {
                var imageUrl = await mediaService.SaveAsync(imageDto, MediaTypes.Image);
                if (string.IsNullOrEmpty(imageUrl))
                {
                    return Result<UpdateProductBasicInfoResponse>.Failure("Failed to upload image",
                        HttpStatusCode.InternalServerError);
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
            await unitOfWork.ProductImagesRepo.AddRangeAsync(newImages);
            product.Images = newImages;
        }

        var currentCategoryIds = product.Categories.Select(x => x.CategoryId).ToList();
        var toAdd = request.CategoryIds.Except(currentCategoryIds).ToList();
        var toRemove = currentCategoryIds.Except(request.CategoryIds).ToList();

        if (toRemove.Any())
        {
            var removed = product.Categories
                .Where(x => toRemove.Contains(x.CategoryId))
                .ToList();

            unitOfWork.ProductCategoriesRepo.RemoveRange(removed);
        }

        if (toAdd.Any())
        {
            var parents = await unitOfWork.CategoryRepo.Entities
                .Where(c => toAdd.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, c => c.ParentCategoryId, cancellationToken);

            var newCategories = toAdd.Select(id => new ProductCategory
            {
                ProductId = product.Id,
                CategoryId = id,
                ParentCategoryId = parents.TryGetValue(id, out var parent) ? parent : null
            }).ToList();

            await unitOfWork.ProductCategoriesRepo.AddRangeAsync(newCategories);
        }

        unitOfWork.ProductsRepo.Update(product);
        await unitOfWork.SaveAsync(cancellationToken);

        var response = product.Adapt<UpdateProductBasicInfoResponse>();
        foreach (var img in response.Images)
        {
            img.ImageUrl = mediaService.GetUrl(img.ImageUrl, MediaTypes.Image)!;
        }

        return Result<UpdateProductBasicInfoResponse>.Success(
            data: response,
            message: "Product info updated successfully",
            statusCode: HttpStatusCode.OK);
    }
}