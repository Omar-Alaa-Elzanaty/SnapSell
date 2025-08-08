using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.Customer.Commands.AddCustomerInformation;

public sealed record AddCustomerInformationCommand(
    string UserId,
    Gender Gender,
    string PhoneNumber,
    DateTime BirthDate,
    List<Guid>FivorateCategoryIds,
    List<Guid>FivorateBrandIds) : IRequest<Result<AddCustomerInfoResult>>;
public sealed record AddCustomerInformationResponse(
    string Id,
    string UserName,
    string Email,
    string PhoneNumber,
    string FirstName,
    string LastName,
    Gender? Gender,
    DateTime? BirthDate);
public record AddCustomerInfoResult(AddCustomerInformationResponse Customer, string Token);