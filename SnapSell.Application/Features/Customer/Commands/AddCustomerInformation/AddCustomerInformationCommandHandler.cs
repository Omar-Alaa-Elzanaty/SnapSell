using System.Net;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces.Authentication;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Application.Features.Customer.Commands.AddCustomerInformation;

internal sealed class AddCustomerInformationCommandHandler(
    IHttpContextAccessor httpContextAccessor,
    IAuthenticationService authenticationService,
    UserManager<Account> userManager,
    IStringLocalizer<AddCustomerInformationCommandHandler> localizer)
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
                message: "canot add Role to Customer",
                statusCode: HttpStatusCode.Forbidden);
        }

        var account = await userManager.FindByIdAsync(userId!);
        
        if (account is null)
        {
            return Result<AddCustomerInformationRespose>.Failure(
                message: localizer["UserNotFound"],
                statusCode: HttpStatusCode.NotFound);
        }
        
        account.Gender = request.Gender;
        account.BirthDate = request.BirthDate;
        
        await userManager.UpdateAsync(account);
        return Result<AddCustomerInformationRespose>.Success(
            data: null!,
            message: localizer["CustomerDetailsAddedSuccessfully"],
            statusCode: HttpStatusCode.Created);
    }
}