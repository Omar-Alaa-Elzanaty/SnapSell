using MediatR;
using SnapSell.Application.DTOs.Product;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.product.Queries.GetAllProductsForSpecificSeller;

public sealed record GetAllProductsForSpecificSellerQuery(
    string SellerId,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PaginatedResult<GetAllProductsForSpecificSellerResponse>>;