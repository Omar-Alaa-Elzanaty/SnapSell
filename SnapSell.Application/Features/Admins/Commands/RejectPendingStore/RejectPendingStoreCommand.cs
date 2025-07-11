using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Admins.Commands.RejectPendingStore
{
    public sealed record RejectPendingStoreCommand(Guid StoreId) : IRequest<Result<bool>>;
}
