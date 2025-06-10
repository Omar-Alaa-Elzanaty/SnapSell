using MediatR;
using Microsoft.AspNetCore.Identity;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Net;
using SnapSell.Application.Interfaces.Authentication;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.Authentication.Commands.RegisterCustomer;

internal sealed class RegisterCustomerCommandHandler(
    IAuthenticationService authenticationService,
    UserManager<Account> userManager,
    RoleManager<IdentityRole> roleManager) : IRequestHandler<RegisterCustomerCommand, Result<RegisterCustomerResult>>
{
    public readonly string DefaultCustomerRole = "Customer";

    public async Task<Result<RegisterCustomerResult>> Handle(RegisterCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var user = new Account
        {
            FullName = request.CutomerName,
            UserName = request.UserName,
            CreatedAt = DateTime.UtcNow,
        };
        if (await userManager.FindByNameAsync(request.UserName) is not null)
        {
            return Result<RegisterCustomerResult>.Failure(
                message: "This Customer Name Is Already taken.",
                statusCode: HttpStatusCode.Conflict);
        }

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors;
            return Result<RegisterCustomerResult>.Failure(
                message: $"Creation Of user proess is failed{errors}",
                statusCode: HttpStatusCode.BadRequest);
        }

        if (!await roleManager.RoleExistsAsync(DefaultCustomerRole))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(DefaultCustomerRole));
            if (!roleResult.Succeeded)
            {
                return Result<RegisterCustomerResult>.Failure(
                    message: "Failed to create customer role",
                    statusCode: HttpStatusCode.BadRequest);
            }
        }

        var addRoleResult = await userManager.AddToRoleAsync(user, DefaultCustomerRole);
        if (!addRoleResult.Succeeded)
        {
            return Result<RegisterCustomerResult>.Failure(
                message: "Failed to assign customer role to user",
                statusCode: HttpStatusCode.InternalServerError);
        }


        var token = await authenticationService.GenerateTokenAsync(user, DefaultCustomerRole, isMobile: true);
        var customerDto = new RegisterResponseCustomerDto(
            user.Id,
            user.FullName,
            user.UserName);

        var registerResult = new RegisterCustomerResult(customerDto, token);

        return Result<RegisterCustomerResult>.Success(
            data: registerResult,
            message: "UserCreated Successfully.",
            HttpStatusCode.OK);
    }
}