using System.Net;
using System.Security.Claims;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.store.Commands.CreateStore;

internal sealed class CreateStoreCommandHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMediaService mediaService)
    : IRequestHandler<CreateStoreCommand, Result<CreateStoreResponse>>
{
    public async Task<Result<CreateStoreResponse>> Handle(CreateStoreCommand request,
        CancellationToken cancellationToken)
    {
        var sellerId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var image = await mediaService.SaveAsync(request.LogoUrl, MediaTypes.Image);

        var existingStore = await unitOfWork.StoresRepo
            .FindAsync(s => s.SellerId == sellerId);

        if (existingStore.Any())
        {
            return Result<CreateStoreResponse>.Failure(
                message: "Seller already has a store.",
                statusCode: HttpStatusCode.Conflict);
        }

        var store = request.Adapt<Store>();
        store.SellerId = sellerId;
        store.LogoUrl = image;

        await unitOfWork.StoresRepo.AddAsync(store);
        await unitOfWork.SaveAsync(cancellationToken);

        var response = store.Adapt<CreateStoreResponse>();
        response.LogoUrl = mediaService.GetUrl(store.LogoUrl, MediaTypes.Image);

        return Result<CreateStoreResponse>.Success(
            data: response,
            message: "Store Created Successfully.",
            statusCode: HttpStatusCode.Created);
    }
}