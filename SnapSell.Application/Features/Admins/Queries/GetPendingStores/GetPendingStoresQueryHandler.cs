using Mapster;
using MediatR;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Extensions;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.Admins.Queries.GetPendingStores
{
    internal class GetPendingStoresQueryHandler : IRequestHandler<GetPendingStoresQuery, PaginatedResult<GetPendingStoresQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPendingStoresQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetPendingStoresQueryDto>> Handle(GetPendingStoresQuery query, CancellationToken cancellationToken)
        {
            var pendingStores = await _unitOfWork.StoresRepo.Entities
                                .Where(x => x.Status == StoreStatusTypes.Pending)
                                .ProjectToType<GetPendingStoresQueryDto>()
                                .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);

            return pendingStores;
        }
    }
}
