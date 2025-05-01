namespace SnapSell.Application.DTOs.Authentication;


public sealed record RegisterRequestCustomerDto(
    string CustomerName,
    string UserName,
    string Password);

public sealed record RegisterResponseCustomerDto(
    string CustomerId,
    string UserName,
    string ShopName);