using MediatR;
using SnapSell.Application.DTOs.Authentication;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Authentication.Queries.CustomerLogIn;

public sealed record CustomerLogInQuery(string UserName, string Password) : IRequest<Result<CustomerLogInResult>>;

public sealed record CustomerLogInResult(LogInCustomerResponse Customer, string Token);