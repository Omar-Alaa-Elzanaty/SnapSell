﻿using MediatR;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.products.Queries.GetAllProductsForSpecificSeller;

public sealed record GetAllProductsForSpecificSellerQuery(
    string SellerId,
    PaginatedRequest Pagination)
    : IRequest<PaginatedResult<GetAllProductsForSpecificSellerResponse>>;

public sealed record GetAllProductsForSpecificSellerResponse(
    Guid ProductId,
    string EnglishName,
    string ArabicName,
    string? EnglishDescription,
    string? ArabicDescription,
    bool IsFeatured,
    bool IsHidden,
    ProductTypes? ProductStatus,
    int MinDeliveryDays,
    int MaxDeliveryDays,
    string? MainImageUrl,
    string? MainVideoUrl,
    ShippingType? ShippingType,
    string BrandName,
    IReadOnlyList<VariantResponseInGetAllProductsToSeller> Variants);

public sealed record VariantResponseInGetAllProductsToSeller(
    Guid VariantId,
    Guid? SizeId,
    Guid? ColorId,
    int Quantity,
    decimal Price,
    decimal RegularPrice,
    decimal? SalePrice,
    string? SKU,
    string? Barcode);