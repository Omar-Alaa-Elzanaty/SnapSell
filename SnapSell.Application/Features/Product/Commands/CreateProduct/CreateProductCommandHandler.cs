using Mapster;
using MediatR;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using System.Net;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Models.MongoDbEntities;

namespace SnapSell.Application.Features.product.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler(
    IUnitOfWork unitOfWork,
    IMediaService mediaService) : IRequestHandler<CreateProductCommand, Result<CreateProductResponse>>
{
    public async Task<Result<CreateProductResponse>> Handle(CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var existingCategories = await unitOfWork.CategoryRepo.Entities
            .Where(c => request.CategoryIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        var allCategoriesExist = request.CategoryIds.All(id => existingCategories.Select(x => x.Id).Contains(id));
        if (!allCategoriesExist)
        {
            return Result<CreateProductResponse>.Failure(
                message: "One or more categories do not exist",
                statusCode: HttpStatusCode.BadRequest);
        }

        var existingBrand = await unitOfWork.BrandsRepo.Entities
            .SingleOrDefaultAsync(x => x.Id == request.BrandId, cancellationToken);
        if (existingBrand is null)
        {
            return Result<CreateProductResponse>.Failure(
                message: $"The Brand Id you Entered is not Valid : {request.BrandId}.",
                statusCode: HttpStatusCode.BadRequest);
        }


        var product = request.Adapt<Product>();
        product.CategoryIds = request.CategoryIds;

        product.Images = new List<ProductImage>();
        foreach (var image in request.Images)
        {
            product.Images.Add(new ProductImage
            {
                ImageUrl = await mediaService.SaveAsync(image, MediaTypes.Image)??"",
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

        await unitOfWork.ProductsRepo.InsertOneAsync(product, cancellationToken);

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