using System.Net;
using Mapster;
using MediatR;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.MongoDbEntities;

namespace SnapSell.Application.Features.product.Commands.AddAdditionalInformationToProduct;

internal sealed class AddAdditionalInformationToProductCommandHandler(
    ISQLBaseRepo<Product> productRepository,
    ISQLBaseRepo<Variant> variantsRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddAdditionalInformationToProductCommand,
        Result<CreateProductAdditionalInformationResponse>>
{
    public async Task<Result<CreateProductAdditionalInformationResponse>> Handle(
        AddAdditionalInformationToProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
        {
            return Result<CreateProductAdditionalInformationResponse>.Failure(
                message: $"No product found with this Id : {request.ProductId}",
                HttpStatusCode.NotFound);
        }

        product.ArabicDescription = request.EnglishDescription;
        product.EnglishDescription = request.ArabicDescription;
        //product.MinDeleveryDays = request.MinDeleveryDays;
        //product.MaxDeleveryDays = request.MaxDeleveryDays;

        //var variantEntities = new List<Variant>();
        //foreach (var variantDto in request.Variants)
        //{
        //    if (variantDto is null) continue;

        //    var variant = new Variant
        //    {
        //        ProductId = request.ProductId,
        //        Color = variantDto.Color,
        //        Size = variantDto.Size,
        //        Quantity = variantDto.Quantity,
        //        Price = variantDto.Price,
        //        SalePrice = variantDto.SalePrice,
        //        Sku = variantDto.Sku ?? GenerateSku(),
        //    };

        //    variantEntities.Add(variant);
        //}

        productRepository.UpdateAsync(product);
        //await variantsRepository.AddRange(variantEntities);
        await unitOfWork.SaveAsync(cancellationToken);

        var response = product.Adapt<CreateProductAdditionalInformationResponse>();
        return Result<CreateProductAdditionalInformationResponse>.Success(
            data: response,
            message: "Product additional information added successfully",
            statusCode: HttpStatusCode.OK);
    }
    private string GenerateSku() => $"SKU-{Guid.NewGuid().ToString()[..8].ToUpper()}";
}