using MediatR;
using SnapSell.Application.DTOs.Product;
using SnapSell.Application.DTOs.variant;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.product.Commands.AddAdditionalInformationToProduct;

public sealed record AddAdditionalInformationToProductCommand(
    Guid ProductId,
    string EnglishDescription,
    string ArabicDescription,
    int MinDeleveryDays,
    int MaxDeleveryDays,
    List<VariantDto?> Variants) : IRequest<Result<CreateProductAdditionalInformationResponse>>;