using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SnapSell.Application.Abstractions.Interfaces.Authentication;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Application.Features.Authentication.Commands.Register;

internal sealed class RegisterCommandHandler(
    IAuthenticationService authenticationService,
    UserManager<Account> userManager) : IRequestHandler<RegisterCommand, Result<RegisterResult>>
{
    public async Task<Result<RegisterResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new Account
        {
            FullName = request.FullName,
            UserName = request.UserName,
            CreatedAt = DateTime.UtcNow,
        };

        if (await userManager.FindByNameAsync(request.UserName) is not null)
        {
            return Result<RegisterResult>.Failure(
                message: "This Seller Name Is Already taken.",
                statusCode: HttpStatusCode.Conflict);
        }

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Result<RegisterResult>.Failure(
                message: "Creation Of user process is failed",
                statusCode: HttpStatusCode.BadRequest);
        }

        var token = await authenticationService.GenerateTokenAsync(user, isMobile: true);
        var response = new RegisterResult(user.Id, user.FullName, user.UserName, token);

        return Result<RegisterResult>.Success(
            data: response,
            message: "User Created Successfully.",
            statusCode: HttpStatusCode.Created);
    }
}