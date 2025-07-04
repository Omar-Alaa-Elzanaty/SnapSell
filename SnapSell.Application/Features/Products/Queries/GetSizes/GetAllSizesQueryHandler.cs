using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Net;
using Mapster;
using SnapSell.Application.Abstractions.Interfaces;

namespace SnapSell.Application.Features.products.Queries.GetSizes;

internal sealed class GetAllSizesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllSizesQuery, Result<IReadOnlyList<GetAllSizesGroupedResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllSizesGroupedResponse>>> Handle(GetAllSizesQuery request,
        CancellationToken cancellationToken)
    {
        var sizes = await unitOfWork.SizesRepo.GetAllAsync();

        var sizeDtos = sizes.Adapt<List<GetAllSizesResponse>>();
        var sizesInMemory = sizeDtos.ToDictionary(s => s.Id);

        var grouped = sizeDtos
            .Where(s => s.ParentSizeId != null)
            .GroupBy(s => s.ParentSizeId!.Value)
            .Select(group =>
            {
                var parent = sizesInMemory.GetValueOrDefault(group.Key);
                return new GetAllSizesGroupedResponse(parent, group.ToList());
            }).ToList();

        return Result<IReadOnlyList<GetAllSizesGroupedResponse>>.Success(
            data: grouped,
            message: "Brands returned Successfully.",
            statusCode: HttpStatusCode.OK);
    }
}