using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using System.Net;

namespace SnapSell.Application.Features.Admins.Commands.RejectPendingStore
{
    internal class RejectPendingStoreCommandHandler : IRequestHandler<RejectPendingStoreCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<RejectPendingStoreCommandHandler> _localizer;

        public RejectPendingStoreCommandHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizer<RejectPendingStoreCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(RejectPendingStoreCommand command, CancellationToken cancellationToken)
        {
            var store = await _unitOfWork.StoresRepo.Entities
            .FirstOrDefaultAsync(x => x.Id == command.StoreId, cancellationToken: cancellationToken);

            if (store==null)
            {
                return Result<bool>.Failure(_localizer["StoreNotFound"], HttpStatusCode.NotFound);
            }

            store.Status = StoreStatusTypes.Rejected;
            _unitOfWork.StoresRepo.Update(store);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Result<bool>.Success(_localizer["StoreRejected"]);
        }
    }
}
