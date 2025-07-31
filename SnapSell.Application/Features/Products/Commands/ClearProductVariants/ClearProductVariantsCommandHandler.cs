using System.Net;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Products.Commands.ClearProductVariants;

internal sealed class ClearProductVariantsCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<ClearProductVariantsCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(ClearProductVariantsCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var product = await unitOfWork.ProductsRepo.Entities
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product is null)
        {
            return Result<Unit>.Failure(
                message: $"Product not found with this id: {request.ProductId}",
                statusCode: HttpStatusCode.NotFound);
        }

        var store = await unitOfWork.StoresRepo.Entities
            .SingleOrDefaultAsync(s => s.Id == product.StoreId && s.AccountId == userId, cancellationToken);

        if (store is null)
        {
            return Result<Unit>.Failure(
                message: "Not authorized to modify this product",
                statusCode: HttpStatusCode.Forbidden);
        }

        unitOfWork.VariantsRepo.RemoveRange(product.Variants);
        await unitOfWork.SaveAsync(cancellationToken);

        return Result<Unit>.Success(
            message: "Product variants cleared successfully",
            statusCode: HttpStatusCode.NoContent);
    }
}