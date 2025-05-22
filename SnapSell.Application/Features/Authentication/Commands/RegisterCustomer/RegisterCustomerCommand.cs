using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Authentication.Commands.RegisterCustomer;

public sealed record RegisterCustomerCommand(string CutomerName,
    string UserName,
    string Password) : IRequest<Result<RegisterCustomerResult>>;

public sealed record RegisterCustomerResult(RegisterResponseCustomerDto Customer, string Token);

public sealed record RegisterResponseCustomerDto(
    string CustomerId,
    string UserName,
    string ShopName);