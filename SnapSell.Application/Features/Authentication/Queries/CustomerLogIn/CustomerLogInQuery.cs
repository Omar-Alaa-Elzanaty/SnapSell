using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Authentication.Queries.CustomerLogIn;

public sealed record CustomerLogInQuery(string UserName, string Password) : IRequest<Result<CustomerLogInResult>>;

public sealed record CustomerLogInResult(LogInCustomerResponse Customer, string Token);

public sealed record LogInCustomerResponse(
    string CustomerId,
    string CustomerName,
    string UserName);