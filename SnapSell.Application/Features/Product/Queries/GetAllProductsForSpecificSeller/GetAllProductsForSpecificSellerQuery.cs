using MediatR;
using SnapSell.Application.DTOs.Product;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.product.Queries.GetAllProductsForSpecificSeller;

public sealed record GetAllProductsForSpecificSellerQuery(
    string SellerId,
    PaginatedRequest Pagination)
    : IRequest<PaginatedResult<GetAllProductsForSpecificSellerResponse>>;