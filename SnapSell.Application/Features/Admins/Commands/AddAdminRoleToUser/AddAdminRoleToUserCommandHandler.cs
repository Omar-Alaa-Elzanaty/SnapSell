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

namespace SnapSell.Application.Features.Admins.Commands.AddAdminRoleToUser;

internal sealed class AddAdminRoleToUserCommandHandler(
    IAuthenticationService authenticationService,
    UserManager<Account> userManager,
    IHttpContextAccessor contextAccessor,
    IStringLocalizer<AddAdminRoleToUserCommandHandler> localizer)
    : IRequestHandler<AddAdminRoleToUserCommand, Result<AddAdminRoleToUserResponse>>
{
    private readonly string _adminRole = "Admin";
    
    public async Task<Result<AddAdminRoleToUserResponse>> Handle(AddAdminRoleToUserCommand request,
        CancellationToken cancellationToken)
    {
        var userId = contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            return Result<AddAdminRoleToUserResponse>.Failure(
                message: localizer["UserNotFound"],
                statusCode: HttpStatusCode.Forbidden);
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result<AddAdminRoleToUserResponse>.Failure(
                message: localizer["UserNotFound"],
                statusCode: HttpStatusCode.NotFound);
        }

        if (await authenticationService.AddRoleToUser(userId,_adminRole) is not true)
        {
            return Result<AddAdminRoleToUserResponse>.Failure(
                message: "Failed to assgien Role to user.",
                statusCode: HttpStatusCode.NotFound);
        }

        return Result<AddAdminRoleToUserResponse>.Success(
            data: user.Adapt<AddAdminRoleToUserResponse>(),
            message: "Role Added Successfully.",
            statusCode: HttpStatusCode.Created);
    }
}