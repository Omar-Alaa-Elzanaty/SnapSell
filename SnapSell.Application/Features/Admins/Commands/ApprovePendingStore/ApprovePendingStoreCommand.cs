using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Admins.Commands.ApprovePendingStore
{
    public sealed record ApprovePendingStoreCommand : IRequest<Result<string>>
    {
        public Guid StoreId { get; init; }

        public ApprovePendingStoreCommand(Guid storeId)
        {
            StoreId = storeId;
        }
    }
}
