using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SnapSell.Application.Abstractions.Interfaces.Authentication;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Application.Features.Authentication.Queries.LogIn;

public sealed class LogInQueryHandler(
    IAuthenticationService authenticationService,
    UserManager<Account> userManager)
    : IRequestHandler<LogInQuery, Result<LogInResult>>
{
    public async Task<Result<LogInResult>> Handle(LogInQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.UserName);
        if (user is null)
        {
            return Result<LogInResult>.Failure(
                message: "This username was not found.",
                statusCode: HttpStatusCode.NotFound);
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            return Result<LogInResult>.Failure(
                message: "Username or password is incorrect.",
                statusCode: HttpStatusCode.Unauthorized);
        }

        var token = await authenticationService.GenerateTokenAsync(user, true);

        var response = new LogInResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.UserName!
        );

        var result = new LogInResult(response, token);

        return Result<LogInResult>.Success(
            data: result,
            message: "User logged in successfully.",
            statusCode: HttpStatusCode.OK);
    }
}