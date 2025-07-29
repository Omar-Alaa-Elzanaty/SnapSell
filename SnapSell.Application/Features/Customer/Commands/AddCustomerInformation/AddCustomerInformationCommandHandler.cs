using System.Net;
using System.Security.Claims;
using Mapster;
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
    : IRequestHandler<AddCustomerInformationCommand, Result<AddCustomerInfoResult>>
{
    private readonly string _defaultCustomerRole = "Customer";

    public async Task<Result<AddCustomerInfoResult>> Handle(AddCustomerInformationCommand request,
        CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var addRoleResult = await authenticationService.AddRoleToUser(userId!, _defaultCustomerRole);
        if (addRoleResult is not true)
        {
            return Result<AddCustomerInfoResult>.Failure(
                message: "canot add Role to Customer",
                statusCode: HttpStatusCode.Forbidden);
        }

        var account = await userManager.FindByIdAsync(userId!);

        if (account is null)
        {
            return Result<AddCustomerInfoResult>.Failure(
                message: localizer["UserNotFound"],
                statusCode: HttpStatusCode.NotFound);
        }

        account.Gender = request.Gender;
        account.BirthDate = request.BirthDate;

        await userManager.UpdateAsync(account);
        var customer = account.Adapt<AddCustomerInformationRespose>();
        var token = await authenticationService.GenerateTokenAsync(account);

        return Result<AddCustomerInfoResult>.Success(
            data: new AddCustomerInfoResult(customer, token),
            message: localizer["CustomerDetailsAddedSuccessfully"],
            statusCode: HttpStatusCode.Created);
    }
}