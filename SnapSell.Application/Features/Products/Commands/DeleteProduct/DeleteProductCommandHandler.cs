using System.Net;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Products.Commands.DeleteProduct;

internal sealed class DeleteProductCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor,
    IMediaService mediaService) : IRequestHandler<DeleteProductCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var product = await unitOfWork.ProductsRepo.Entities
            .Include(p => p.Images)
            .Include(p => p.Variants)
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product is null)
        {
            return Result<Unit>.Failure(
                message: "Product not found",
                statusCode: HttpStatusCode.NotFound);
        }

        var store = await unitOfWork.StoresRepo.Entities
            .FirstOrDefaultAsync(s => s.Id == product.StoreId && s.AccountId == userId, cancellationToken);

        if (store is null)
        {
            return Result<Unit>.Failure(
                message: "Not authorized to delete this product",
                statusCode: HttpStatusCode.Forbidden);
        }

        foreach (var image in product.Images)
        {
            mediaService.Delete(image.ImageUrl);
        }

        if (product.HasVariants)
        {
            unitOfWork.VariantsRepo.RemoveRange(product.Variants);
        }

        unitOfWork.ProductCategoriesRepo.RemoveRange(product.Categories);
        unitOfWork.ProductImagesRepo.RemoveRange(product.Images);

        unitOfWork.ProductsRepo.Remove(product);
        await unitOfWork.SaveAsync(cancellationToken);

        return Result<Unit>.Success(
            message: "Product deleted successfully",
            statusCode: HttpStatusCode.NoContent);
    }
}