using Mapster;
using MediatR;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Extensions;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.Admins.Queries.GetPendingStores;

internal sealed class GetPendingStoresQueryHandler(IUnitOfWork unitOfWork, IMediaService mediaService)
    : IRequestHandler<GetPendingStoresQuery, PaginatedResult<GetPendingStoresQueryDto>>
{
    public async Task<PaginatedResult<GetPendingStoresQueryDto>> Handle(GetPendingStoresQuery query,
        CancellationToken cancellationToken)
    {
        var result = await unitOfWork.StoresRepo.Entities
            .Where(x => x.Status == StoreStatusTypes.Pending)
            .ProjectToType<GetPendingStoresQueryDto>()
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);

        var items = result.Data?.Items;
        if (items == null || items.Count == 0)
            return result;

        foreach (var store in items)
        {
            store.LogoUrl = mediaService.GetUrl(store.LogoUrl, MediaTypes.Image);
        }

        return result;
    }
}