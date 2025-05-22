using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Admins.Commands.ApproveBendingStore
{
    public sealed record ApproveBendingStoreCommand : IRequest<Result<string>>
    {
        public string StoreId { get; set; }
    }

    internal class ApproveBendingStoreCommandHandler : IRequestHandler<ApproveBendingStoreCommand, Result<string>>
    {
        public Task<Result<string>> Handle(ApproveBendingStoreCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
