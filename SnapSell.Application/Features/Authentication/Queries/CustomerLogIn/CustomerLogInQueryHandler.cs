using MediatR;
using Microsoft.AspNetCore.Identity;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
using System.Net;
using SnapSell.Application.Abstractions.Interfaces.Authentication;

namespace SnapSell.Application.Features.Authentication.Queries.CustomerLogIn;

public sealed class CustomerLogInQueryHandler(
    IAuthenticationService authenticationService,
    SignInManager<Account> signInManager,
    UserManager<Account> userManager) : IRequestHandler<CustomerLogInQuery, Result<CustomerLogInResult>>
{
    public async Task<Result<CustomerLogInResult>> Handle(CustomerLogInQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.UserName);
        if (user is null)
        {
            return Result<CustomerLogInResult>.Failure(
                message: "This UserName Is not found.",
                statusCode: HttpStatusCode.NotFound);
        }

        var result = await userManager.CheckPasswordAsync(user, request.Password);
        if (result is not true)
        {
            return Result<CustomerLogInResult>.Failure(
                message: "the UserName or passwored is wrong.",
                statusCode: HttpStatusCode.Unauthorized);
        }

        var token = await authenticationService.GenerateTokenAsync(user, true);
        var customer = new LogInCustomerResponse(user.Id, user.FullName, user.UserName!);
        var logInResult = new CustomerLogInResult(customer, token);

        return Result<CustomerLogInResult>.Success(
            data: logInResult,
            message: "UserCreated Successfully.",
            HttpStatusCode.OK);
    }
}