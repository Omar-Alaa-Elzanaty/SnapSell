namespace SnapSell.Application.DTOs.Authentication;

public sealed record LogInSellerResponse(
    string SellerId,
    string SellerName,
    string ShopName);


public sealed record LogInSellerRequest(string ShopName, string Password);