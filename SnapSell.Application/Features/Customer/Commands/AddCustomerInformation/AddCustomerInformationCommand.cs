using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.Customer.Commands.AddCustomerInformation;

public sealed record AddCustomerInformationCommand(
    string UserId,
    Gender Gender,
    DateTime BirthDate) : IRequest<Result<AddCustomerInfoResult>>;

public sealed record AddCustomerInformationRespose(
    string Id,
    Gender Gender,
    DateTime BirthDate);

public record AddCustomerInfoResult(AddCustomerInformationRespose Customer, string Token);