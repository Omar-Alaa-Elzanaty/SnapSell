using MediatR;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Net;
using Mapster;

namespace SnapSell.Application.Features.product.Queries.GetSizes;

internal sealed class GetAllSizesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllSizesQuery, Result<IReadOnlyList<GetAllSizesResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllSizesResponse>>> Handle(GetAllSizesQuery request,
        CancellationToken cancellationToken)
    {
        var sizes = await unitOfWork.SizesRepo.GetAllAsync();
        return Result<IReadOnlyList<GetAllSizesResponse>>.Success(
            data: sizes.Adapt<IReadOnlyList<GetAllSizesResponse>>(),
            message: "Brands returned Successfully.",
            HttpStatusCode.OK);
    }
}