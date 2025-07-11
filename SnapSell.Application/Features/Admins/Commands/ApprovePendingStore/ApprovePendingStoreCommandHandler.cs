using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using System.Net;
using SnapSell.Application.Abstractions.Interfaces;
using MongoDB.Bson.Serialization.Conventions;

namespace SnapSell.Application.Features.Admins.Commands.ApprovePendingStore;

internal class ApprovePendingStoreCommandHandler : IRequestHandler<ApprovePendingStoreCommand, Result<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApprovePendingStoreCommandHandler> _localizer;

    public ApprovePendingStoreCommandHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizer<ApprovePendingStoreCommandHandler> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<string>> Handle(ApprovePendingStoreCommand command, CancellationToken cancellationToken)
    {
        var store = await _unitOfWork.StoresRepo.Entities
            .FirstOrDefaultAsync(x => x.Id == command.StoreId, cancellationToken);

        if (store == null)
        {
            return Result<string>.Failure(_localizer["StoreNotFound"], HttpStatusCode.NotFound);
        }

        store.Status = StoreStatusTypes.Verified;
        _unitOfWork.StoresRepo.Update(store);

        await _unitOfWork.SaveAsync(cancellationToken);

        return Result<string>.Success(_localizer["StoreApproved"]);
    }
}