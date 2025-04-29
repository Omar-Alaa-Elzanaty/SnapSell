namespace SnapSell.Application.DTOs.Authentication;

public sealed record RegisterRequestSellerDto(
    string SellerName,
    string ShopName,
    string Password);

public sealed record RegisterResponseSellerDto(
    string SellerId,
    string SellerName,
    string ShopName);