namespace SnapSell.Application.DTOs.Product;

public sealed record GetAllProductsRequest(
    int PageNumber,
    int PageSize);