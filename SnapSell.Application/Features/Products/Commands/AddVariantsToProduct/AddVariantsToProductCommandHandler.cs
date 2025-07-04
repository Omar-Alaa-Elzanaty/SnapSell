using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Net;
using System.Security.Claims;
using SnapSell.Application.Abstractions.Interfaces;

namespace SnapSell.Application.Features.products.Commands.AddVariantsToProduct;

internal sealed class AddVariantsToProductCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<AddVariantsToProductCommand, Result<List<AddVariantsToProductResponse>>>
{
    public async Task<Result<List<AddVariantsToProductResponse>>> Handle(AddVariantsToProductCommand request,
        CancellationToken cancellationToken)
    {
        var currentUser = httpContextAccessor.HttpContext?.User;
        if (currentUser is null)
        {
            return Result<List<AddVariantsToProductResponse>>.Failure(
                message: "Current user is null",
                HttpStatusCode.Unauthorized);
        }

        var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<List<AddVariantsToProductResponse>>.Failure(
                message: "User ID not found in claims",
                HttpStatusCode.Unauthorized);
        }

        //Variants = request.Variants?.Select(v => new Variant
        //{
        //    SizeId = v.SizeId,
        //    ColorId = v.ColorId,
        //    Quantity = v.Quantity,
        //    Price = v.Price,
        //    RegularPrice = v.RegularPrice,
        //    SalePrice = v.SalePrice,
        //    SKU = v.SKU ?? GenerateSku(),
        //    Barcode = v.Barcode ?? GenerateBarcode()
        //}).ToList() ?? new List<Variant>()
        return Result<List<AddVariantsToProductResponse>>.Failure(
            message: "User ID not found in claims",
            HttpStatusCode.Unauthorized);
    }
}