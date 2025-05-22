using MediatR;
using Microsoft.AspNetCore.Identity;
using SnapSell.Application.Interfaces.Authentication;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models;
using System.Net;

namespace SnapSell.Application.Features.Authentication.Queries.CustomerLogIn;

public sealed class CustomerLogInQueryHandler(
    IAuthenticationService authenticationService,
    SignInManager<User> signInManager,
    UserManager<User> userManager) : IRequestHandler<CustomerLogInQuery, Result<CustomerLogInResult>>
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
        var lgInResult = new CustomerLogInResult(customer, token);

        return Result<CustomerLogInResult>.Success(
            data: lgInResult,
            message: "UserCreated Successfully.",
            HttpStatusCode.OK);
    }
}