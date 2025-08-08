using System.Net;
using System.Security.Claims;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Abstractions.Interfaces.Authentication;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Application.Features.Customer.Commands.AddCustomerInformation;

internal sealed class AddCustomerInformationCommandHandler(
    IHttpContextAccessor httpContextAccessor,
    IAuthenticationService authenticationService,
    UserManager<Account> userManager,
    IUnitOfWork unitOfWork,
    IStringLocalizer<AddCustomerInformationCommandHandler> localizer)
    : IRequestHandler<AddCustomerInformationCommand, Result<AddCustomerInfoResult>>
{
    private readonly string _defaultCustomerRole = "Customer";

    public async Task<Result<AddCustomerInfoResult>> Handle(AddCustomerInformationCommand request,
        CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<AddCustomerInfoResult>.Failure(
                message:"The Authentication is required",
                statusCode:HttpStatusCode.BadRequest);
        }

        var addRoleResult = await authenticationService.AddRoleToUser(userId, _defaultCustomerRole);
        if (!addRoleResult)
        {
            return Result<AddCustomerInfoResult>.Failure(
                message:"Cannot add role to user",
                statusCode:HttpStatusCode.Forbidden);
        }

        var account = await userManager.FindByIdAsync(userId);
        if (account is null)
        {
            return Result<AddCustomerInfoResult>.Failure(
                message:localizer["UserNotFound"],
                statusCode:HttpStatusCode.NotFound);
        }

        if (!await userManager.HasPasswordAsync(account))
        {
            return Result<AddCustomerInfoResult>.Failure(
                message:"User must register first",
                statusCode:HttpStatusCode.Forbidden);
        }

        var categoryIds = request.FivorateCategoryIds.Distinct().ToHashSet();
        var brandIds = request.FivorateBrandIds.Distinct().ToHashSet();
        
        if (categoryIds.Count > 0)
        {
            var validCategoryIds = await unitOfWork.CategoryRepo.Entities
                .Where(c => categoryIds.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            var invalidCategoryIds = categoryIds.Except(validCategoryIds).ToList();
            if (invalidCategoryIds.Any())
            {
                return Result<AddCustomerInfoResult>.Failure(
                    message:$"Invalid Category IDs: {string.Join(", ", invalidCategoryIds)}",
                    statusCode:HttpStatusCode.BadRequest);
            }

            var existingCategoryIds = await unitOfWork.ClientCategoryFavoriteRepo.Entities
                .Where(f => f.AccountId == account.Id)
                .Select(f => f.CategoryId)
                .ToListAsync(cancellationToken);

            var newCategoryFavorites = validCategoryIds
                .Except(existingCategoryIds)
                .Select(id => new ClientCategoryFavorite
                {
                    AccountId = account.Id,
                    CategoryId = id
                }).ToList();

            if (newCategoryFavorites.Any())
            {
                await unitOfWork.ClientCategoryFavoriteRepo.AddRange(newCategoryFavorites);
            }
        }
        
        if (brandIds.Count > 0)
        {
            var validBrandIds = await unitOfWork.BrandsRepo.Entities
                .Where(b => brandIds.Contains(b.Id))
                .Select(b => b.Id)
                .ToListAsync(cancellationToken);

            var invalidBrandIds = brandIds.Except(validBrandIds).ToList();
            if (invalidBrandIds.Any())
            {
                return Result<AddCustomerInfoResult>.Failure(
                    message:$"Invalid Brand IDs: {string.Join(", ", invalidBrandIds)}",
                    statusCode:HttpStatusCode.BadRequest);
            }

            var existingBrandIds = await unitOfWork.ClientBrandFavoriteRepo.Entities
                .Where(f => f.AccountId == account.Id)
                .Select(f => f.BrandId)
                .ToListAsync(cancellationToken);

            var newBrandFavorites = validBrandIds
                .Except(existingBrandIds)
                .Select(id => new ClientBrandFavorite
                {
                    AccountId = account.Id,
                    BrandId = id
                }).ToList();

            if (newBrandFavorites.Any())
            {
                await unitOfWork.ClientBrandFavoriteRepo.AddRange(newBrandFavorites);
            }
        }
        
        account.Gender = request.Gender;
        account.BirthDate = request.BirthDate;
        account.PhoneNumber = request.PhoneNumber;

        await userManager.UpdateAsync(account);
        await unitOfWork.SaveAsync(cancellationToken);

        var response = account.Adapt<AddCustomerInformationResponse>();
        var token = await authenticationService.GenerateTokenAsync(account);

        return Result<AddCustomerInfoResult>.Success(
            data:new AddCustomerInfoResult(response, token),
            message:localizer["CustomerDetailsAddedSuccessfully"],
            statusCode:HttpStatusCode.Created);
    }
}
