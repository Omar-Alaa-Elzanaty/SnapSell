using System.Net;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Abstractions.Interfaces.Authentication;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Customer.Commands.AddCustomerInformation;

internal sealed class AddCustomerInformationCommandHandler(
    IHttpContextAccessor httpContextAccessor,
    IAuthenticationService authenticationService)
    : IRequestHandler<AddCustomerInformationCommand, Result<AddCustomerInformationRespose>>
{
    private readonly string _defaultCustomerRole = "Customer";
    public async Task<Result<AddCustomerInformationRespose>> Handle(AddCustomerInformationCommand request,
        CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var addRoleResult = await authenticationService.AddRoleToUser(userId!, _defaultCustomerRole);
        if (addRoleResult is not true)
        {
            return Result<AddCustomerInformationRespose>.Failure(
                message: "canot add Role to Customer" ,
                statusCode:HttpStatusCode.Forbidden);
        }

        return Result<AddCustomerInformationRespose>.Success(
            data:null!,
            message: "Customer Details Added successfuly." ,
            statusCode:HttpStatusCode.Created);
    }
}