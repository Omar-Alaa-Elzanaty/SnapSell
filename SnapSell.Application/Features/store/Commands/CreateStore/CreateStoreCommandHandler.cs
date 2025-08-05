using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Abstractions.Interfaces.Authentication;
using SnapSell.Domain.Constants;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
using System.Net;
using System.Security.Claims;

namespace SnapSell.Application.Features.store.Commands.CreateStore;

internal sealed class CreateStoreCommandHandler(
    IAuthenticationService authenticationService,
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMediaService mediaService,
    UserManager<Account> userManager,
    IStringLocalizer<CreateStoreCommandHandler> localizer)
    : IRequestHandler<CreateStoreCommand, Result<CreateStoreResult>>
{
    public async Task<Result<CreateStoreResult>> Handle(CreateStoreCommand request,
        CancellationToken cancellationToken)
    {
        var sellerId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var image = await mediaService.SaveAsync(request.LogoUrl, MediaTypes.Image);

        var seller = await userManager.FindByIdAsync(sellerId);
        if (seller is null)
        {
            return Result<CreateStoreResult>.Failure(
                message: localizer["SellerNotFound"],
                statusCode: HttpStatusCode.NotFound);
        }
        
        if (!await userManager.HasPasswordAsync(seller))
        {
            return Result<CreateStoreResult>.Failure(
                message: "user must rigester first",
                statusCode: HttpStatusCode.NotFound);
        }

        var existingStore = await unitOfWork.StoresRepo
            .FindAsync(s => s.AccountId == sellerId);

        if (existingStore.Any())
        {
            return Result<CreateStoreResult>.Failure(
                message: "Seller already has a store.",
                statusCode: HttpStatusCode.Conflict);
        }
        var isaValidStoreId = await unitOfWork.StoresRepo
            .FindAsync(s => s.StoreId == request.StoreId);
        
        if (isaValidStoreId.Any()) 
        {
            return Result<CreateStoreResult>.Failure(
                message: "This StoreId is already taken, please use another one.",
                statusCode: HttpStatusCode.Conflict); 
        }

        var store = request.Adapt<Store>();
        store.AccountId = sellerId;
        store.LogoUrl = image;

        var result = await authenticationService.AddRoleToUser(sellerId, Roles.Seller);
        if (result is not true)
        {
            return Result<CreateStoreResult>.Failure(
                message: "cannot add seller role to user.",
                statusCode: HttpStatusCode.BadRequest);
        }

        await unitOfWork.StoresRepo.AddAsync(store);
        await unitOfWork.SaveAsync(cancellationToken);

        var storeData = store.Adapt<CreateStoreResponse>();
        storeData.LogoUrl = mediaService.GetUrl(store.LogoUrl, MediaTypes.Image);
        var token = await authenticationService.GenerateTokenAsync(seller);
        
        return Result<CreateStoreResult>.Success(
            data: new CreateStoreResult(storeData, token),
            message: "Store Created Successfully.",
            statusCode: HttpStatusCode.Created);
    }
}