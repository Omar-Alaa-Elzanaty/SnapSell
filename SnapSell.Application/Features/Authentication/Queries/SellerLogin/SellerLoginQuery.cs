﻿using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Authentication.Queries.SellerLogin;

public sealed record SellerLoginQuery(string ShopName, string Password) : IRequest<Result<SellerLogInResult>>;

public sealed record SellerLogInResult(LogInSellerResponse Seller,string Token);

public sealed record LogInSellerResponse(
    string SellerId,
    string SellerName,
    string ShopName);