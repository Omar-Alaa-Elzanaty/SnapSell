using System.Net;
using Mapster;
using MediatR;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Features.store.Commands.CreateStore;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.store.Queries.GetStoreById;

internal sealed class GetStoreByIdQueryHandler(
    IUnitOfWork unitOfWork,
    IStringLocalizer<GetStoreByIdQueryHandler> _localizer)
    : IRequestHandler<GetStoreByIdQuery, Result<GetStoreByIdResponse>>
{
    public async Task<Result<GetStoreByIdResponse>> Handle(GetStoreByIdQuery request,
        CancellationToken cancellationToken)
    {
        var store =await unitOfWork.StoresRepo.FindOnCriteriaAsync(i => i.Id == request.Id);

        if (store == null)
        {
            return new Result<GetStoreByIdResponse>
            {
                Message = "No store found",
                StatusCode = HttpStatusCode.NotFound
            };
        }


        var response = store.Adapt<GetStoreByIdResponse>();

        return Result<GetStoreByIdResponse>.Success(response);

    }
}