using MediatR;
using SnapSell.Application.DTOs.Product;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.product.Commands.CreateProduct;

public sealed record CreatProductCommand(Guid BrandId,
    string EnglishName,
    string ArabicName,
    bool IsFeatured,
    bool IsHidden,
    ShippingType ShippingType,
    ProductStatus ProductStatus) : IRequest<Result<CreateProductResponse>>;