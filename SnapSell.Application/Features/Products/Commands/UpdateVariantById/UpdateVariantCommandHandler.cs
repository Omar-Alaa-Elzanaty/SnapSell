using System.Net;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Products.Commands.UpdateVariantById;

internal sealed class UpdateVariantCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<UpdateVariantCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateVariantCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var variant = await unitOfWork.VariantsRepo.Entities
            .Include(v => v.Product)
            .ThenInclude(p => p.Store)
            .FirstOrDefaultAsync(v => v.Id == request.VariantId, cancellationToken);

        if (variant is null)
        {
            return Result<Unit>.Failure(
                message: "Variant not found",
                statusCode: HttpStatusCode.NotFound);
        }

        if (variant.Product.Store.AccountId != userId)
        {
            return Result<Unit>.Failure(
                message: "Unauthorized to update this variant",
                statusCode: HttpStatusCode.Forbidden);
        }

        var sizeExists = await unitOfWork.SizesRepo.Entities
            .AnyAsync(s => s.Id == request.SizeId, cancellationToken);

        if (!sizeExists)
        {
            return Result<Unit>.Failure(
                message: "Invalid SizeId",
                statusCode: HttpStatusCode.NotFound);
        }

        variant.Price = request.Price;
        variant.SalePrice = request.SalePrice;
        variant.CostPrice = request.CostPrice;
        variant.Quantity = request.Quantity;
        variant.SizeId = request.SizeId;

        unitOfWork.VariantsRepo.Update(variant);
        await unitOfWork.SaveAsync(cancellationToken);

        return Result<Unit>.Success(
            message:"Variant updated successfully",
            statusCode:HttpStatusCode.NoContent);
    }
}