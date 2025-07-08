using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.Customer.Commands.AddCustomerInformation;

public sealed record AddCustomerInformationCommand(
    string UserId,
    Gender Gender, 
    DateTime BirthDate):IRequest<Result<AddCustomerInformationRespose>>;

public sealed record AddCustomerInformationRespose(string UserId,
    Gender Gender, 
    DateTime BirthDate);