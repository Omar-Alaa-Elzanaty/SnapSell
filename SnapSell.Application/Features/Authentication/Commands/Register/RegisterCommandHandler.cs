using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces.Authentication;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
using System.Net;
using System.Security.Claims;

namespace SnapSell.Application.Features.Authentication.Commands.Register;

internal sealed class RegisterCommandHandler(
    IAuthenticationService authenticationService,
    UserManager<Account> userManager,
    IHttpContextAccessor contextAccessor,
    IMapper mapper,
    IStringLocalizer<RegisterCommandHandler> localizer) : IRequestHandler<RegisterCommand, Result<RegisterResult>>
{
    public async Task<Result<RegisterResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userId = contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            return Result<RegisterResult>.Failure(
                message: localizer["UserNotFound"],
                statusCode: HttpStatusCode.Forbidden);
        }

        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return Result<RegisterResult>.Failure(
                message: localizer["UserNotFound"],
                statusCode: HttpStatusCode.NotFound);
        }

        if (await userManager.FindByNameAsync(request.UserName) is not null)
        {
            return Result<RegisterResult>.Failure(
                message: localizer["UserNameUsedBefore"],
                statusCode: HttpStatusCode.Conflict);
        }

        mapper.Map(request, user);

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return Result<RegisterResult>.ValidationFailure(result.Errors);
        }

        result = await userManager.AddPasswordAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return Result<RegisterResult>.ValidationFailure(result.Errors);
        }

        var response = new RegisterResult()
        {
            Token = await authenticationService.GenerateTokenAsync(user, isMobile: true),
            UserInfo = mapper.Map<RegisterUserInfo>(user)
        };

        return Result<RegisterResult>.Success(
            data: response,
            message: localizer["UserCreatedSuccessfully"],
            statusCode: HttpStatusCode.Created);
    }
}