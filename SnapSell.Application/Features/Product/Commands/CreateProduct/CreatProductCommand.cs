using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.DTOs.Product;
using SnapSell.Application.DTOs.variant;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.product.Commands.CreateProduct;

public sealed record CreatProductCommand(
    string EnglishName,
    string ArabicName,
    string? Description,
    string? ShortDescription,
    bool IsFeatured,
    bool IsHidden,
    int MinDeleveryDays,
    int MaxDeleveryDays,
    IFormFile MainImageUrl,
    IFormFile MainVideoUrl,
    List<VariantDto> Variants) : IRequest<Result<CreateProductResponse>>;