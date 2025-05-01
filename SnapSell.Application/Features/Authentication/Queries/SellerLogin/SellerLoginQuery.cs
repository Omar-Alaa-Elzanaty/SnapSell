using MediatR;
using SnapSell.Application.DTOs.Authentication;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Authentication.Queries.SellerLogin;

public sealed record SellerLoginQuery(string ShopName, string Password) : IRequest<Result<SellerLogInResult>>;

public sealed record SellerLogInResult(LogInSellerResponse Seller,string Token);