using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SnapSell.Application.Interfaces.Authentication;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Application.Features.Authentication.Queries.SellerLogin;

internal sealed class SellerLoginQueryHandler(
    IAuthenticationService authenticationService,
    SignInManager<Account> signInManager,
    UserManager<Account> userManager) : IRequestHandler<SellerLoginQuery, Result<SellerLogInResult>>
{
    public async Task<Result<SellerLogInResult>> Handle(SellerLoginQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.ShopName);
        if (user is null)
        {
            return Result<SellerLogInResult>.Failure(
                message: "This Shop Name Is not found.",
                statusCode: HttpStatusCode.NotFound);
        }

        var result = await userManager.CheckPasswordAsync(user, request.Password);
        if (result is not true)
        {
            return Result<SellerLogInResult>.Failure(
                message: "the UserName or passwored is wrong.",
                statusCode: HttpStatusCode.Unauthorized);
        }

        var token = await authenticationService.GenerateTokenAsync(user, true);
        var seller = new LogInSellerResponse(user.Id, user.FullName, user.UserName!);
        var lgInResult = new SellerLogInResult(seller, token);

        return Result<SellerLogInResult>.Success(
            data: lgInResult,
            message: "UserCreated Successfully.",
            HttpStatusCode.OK);
    }
}