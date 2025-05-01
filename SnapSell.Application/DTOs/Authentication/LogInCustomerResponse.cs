namespace SnapSell.Application.DTOs.Authentication;

public sealed record LogInCustomerResponse(
    string CustomerId,
    string CustomerName,
    string UserName);

public sealed record LogInCustomerRequest(string UserName, string Password);