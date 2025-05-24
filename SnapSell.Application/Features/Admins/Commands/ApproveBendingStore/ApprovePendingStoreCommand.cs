using MediatR;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Admins.Commands.ApproveBendingStore
{
    public sealed record ApprovePendingStoreCommand : IRequest<Result<string>>
    {
        public string StoreId { get; init; }

        public ApprovePendingStoreCommand(string storeId)
        {
            StoreId = storeId;
        }
    }
}
