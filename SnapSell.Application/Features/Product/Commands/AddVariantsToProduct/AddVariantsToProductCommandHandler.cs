using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.DTOs.variant;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models;
using System.Net;
using System.Security.Claims;
using FluentValidation;
using SnapSell.Domain.Extnesions;

namespace SnapSell.Application.Features.product.Commands.AddVariantsToProduct;

internal sealed class AddVariantsToProductCommandHandler(
    ISQLBaseRepo<Product> productRepository,
    ISQLBaseRepo<Variant> variantRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<AddVariantsToProductCommand, Result<List<AddVariantsToProductResponse>>>
{
    public async Task<Result<List<AddVariantsToProductResponse>>> Handle(AddVariantsToProductCommand request,
        CancellationToken cancellationToken)
    {
        //var validationResult = await validator.ValidateAsync(request, cancellationToken);

        //if (!validationResult.IsValid)
        //{
        //    var errors = validationResult.Errors.GetErrorsDictionary();
        //    return new Result<List<AddVariantsToProductResponse>>()
        //    {
        //        Errors = errors,
        //        StatusCode = HttpStatusCode.BadRequest,
        //        Message = "Validation failed"
        //    };
        //}

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