using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Authentication.Commands.RegisterSeller;

public sealed record RegisterSellerCommand(string SellerName,
    string ShopName,
    string Password) : IRequest<Result<RegisterSellerResult>>;

public sealed record RegisterSellerResult(RegisterResponseSellerDto Seller , string Token);

public sealed record RegisterResponseSellerDto(
    string SellerId,
    string SellerName,
    string ShopName);