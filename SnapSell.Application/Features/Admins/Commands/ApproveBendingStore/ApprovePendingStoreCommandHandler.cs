using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using System.Net;
namespace SnapSell.Application.Features.Admins.Commands.ApproveBendingStore
{
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
            var isStoreFound = await _unitOfWork.StoresRepo.Entites
                             .AnyAsync(x => x.Id == command.StoreId, cancellationToken: cancellationToken);

            if (!isStoreFound)
            {
                return Result<string>.Failure(_localizer["StoreNotFound"], HttpStatusCode.NotFound);
            }

            await EntityFrameworkQueryableExtensions.ExecuteUpdateAsync(
                _unitOfWork.StoresRepo.Entites.Where(x => x.Id == command.StoreId),
                x => x.SetProperty(x => x.Status, StoreStatusTypes.Verified),
                cancellationToken
            );

            return Result<string>.Success(_localizer["StoreApproved"]);
        }
    }
}
