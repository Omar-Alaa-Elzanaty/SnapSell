using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SnapSell.Application.Interfaces.Authentication;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.Authentication.Commands.RegisterSeller;

internal sealed class RegisterSellerCommandHandler(
    IAuthenticationService authenticationService,
    UserManager<Account> userManager,
    RoleManager<IdentityRole> roleManager)
    : IRequestHandler<RegisterSellerCommand, Result<RegisterSellerResult>>
{
    public readonly string DefaultSellerRole = "Seller";

    public async Task<Result<RegisterSellerResult>> Handle(RegisterSellerCommand request,
        CancellationToken cancellationToken)
    {
        var user = new Account
        {
            FullName = request.SellerName,
            UserName = request.ShopName,
            CreatedAt = DateTime.UtcNow,
        };
        if (await userManager.FindByNameAsync(request.ShopName) is not null)
        {
            return Result<RegisterSellerResult>.Failure(
                message: "This Seller Name Is Already taken.",
                statusCode: HttpStatusCode.Conflict);
        }

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Result<RegisterSellerResult>.Failure(
                message: "Creation Of user process is failed",
                statusCode: HttpStatusCode.BadRequest);
        }

        if (!await roleManager.RoleExistsAsync(DefaultSellerRole))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(DefaultSellerRole));
            if (!roleResult.Succeeded)
            {
                return Result<RegisterSellerResult>.Failure(
                    message: "Failed to create seller role",
                    statusCode: HttpStatusCode.BadRequest);
            }
        }

        var addRoleResult = await userManager.AddToRoleAsync(user, DefaultSellerRole);
        if (!addRoleResult.Succeeded)
        {
            return Result<RegisterSellerResult>.Failure(
                message: "Failed to assign seller role to user",
                statusCode: HttpStatusCode.InternalServerError);
        }

        var token = await authenticationService.GenerateTokenAsync(user, DefaultSellerRole, isMobile: true);
        var sellerDto = new RegisterResponseSellerDto(
            user.Id,
            user.FullName,
            user.UserName);

        var registerResult = new RegisterSellerResult(sellerDto, token);

        return Result<RegisterSellerResult>.Success(
            data: registerResult,
            message: "UserCreated Successfully.",
            HttpStatusCode.OK);
    }
}