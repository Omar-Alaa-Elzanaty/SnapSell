using System.Net;
using System.Security.Claims;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.Products.Commands.UpdateProductVariants;

internal sealed class UpdateProductVariantsCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<UpdateProductVariantsCommand, Result<UpdateProductVariantsResponse>>
{
    public async Task<Result<UpdateProductVariantsResponse>> Handle(
        UpdateProductVariantsCommand request,
        CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Result<UpdateProductVariantsResponse>.Failure(
                message:"Unauthorized",
                statusCode:HttpStatusCode.Unauthorized);
        }

        var product = await unitOfWork.ProductsRepo.Entities
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            return Result<UpdateProductVariantsResponse>.Failure(
                message:"Product not found",
                statusCode:HttpStatusCode.NotFound);
        }

        var store = await unitOfWork.StoresRepo.Entities
            .SingleOrDefaultAsync(s => s.AccountId == userId, cancellationToken);

        if (store == null || product.StoreId != store.Id)
        {
            return Result<UpdateProductVariantsResponse>.Failure(
                message:"Unauthorized access",
                statusCode:HttpStatusCode.Forbidden);
        }

        if (request.HasVariants && request.Variants.Count(v => v.IsDefault) != 1)
        {
            return Result<UpdateProductVariantsResponse>.Failure(
                message: "Exactly one variant must be default",
                statusCode: HttpStatusCode.BadRequest);
        }

        product.HasVariants = request.HasVariants;
        var now = DateTime.UtcNow;

        unitOfWork.VariantsRepo.RemoveRange(product.Variants);
        product.Variants.Clear();

        if (request.HasVariants)
        {
            var validSizeIds = await unitOfWork.SizesRepo.Entities
                .Select(s => s.Id)
                .ToListAsync(cancellationToken);

            var invalidSize = request.Variants.FirstOrDefault(v => !validSizeIds.Contains(v.SizeId));
            if (invalidSize != null)
            {
                return Result<UpdateProductVariantsResponse>.Failure(
                    message: $"Invalid size ID: {invalidSize.SizeId}",
                    statusCode: HttpStatusCode.BadRequest);
            }

            var newVariants = request.Variants.Select(variantDto =>
            {
                var variant = variantDto.Adapt<Variant>();
                variant.Id = Guid.NewGuid();
                variant.ProductId = product.Id;
                variant.CreatedAt = now;
                return variant;
            }).ToList();

            await unitOfWork.VariantsRepo.AddRangeAsync(newVariants);
            product.Variants = newVariants;
        }
        else
        {
            if (request.Variants.Count != 1)
            {
                return Result<UpdateProductVariantsResponse>.Failure(
                    message: "Simple product must have exactly one variant",
                    statusCode: HttpStatusCode.BadRequest);
            }

            var mainVariant = request.Variants.First();
            product.Price = mainVariant.Price;
            product.SalePrice = mainVariant.SalePrice;
            product.CostPrice = mainVariant.CostPrice;
            product.Quantity = mainVariant.Quantity;
            product.Sku = mainVariant.Sku;
        }

        unitOfWork.ProductsRepo.Update(product);
        await unitOfWork.SaveAsync(cancellationToken);

        var response = new UpdateProductVariantsResponse
        {
            ProductId = product.Id,
            Variants = product.Variants.Adapt<List<UpdateProductVariantResponse>>()
        };

        return Result<UpdateProductVariantsResponse>.Success(
            data: response,
            message: "Product variants replaced successfully",
            statusCode: HttpStatusCode.OK);
    }
}