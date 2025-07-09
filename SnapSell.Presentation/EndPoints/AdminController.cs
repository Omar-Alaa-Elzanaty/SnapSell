using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.Admins.Commands.ApprovePendingStore;
using SnapSell.Application.Features.Admins.Commands.RejectPendingStore;
using SnapSell.Application.Features.Admins.Queries.GetPendingStores;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints;

[Authorize(Roles = "Admin")]
public sealed class AdminController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator) 
    {
        _mediator = mediator;
    }
    [HttpPut("ApprovePendingStore/{storeId}")]
    public async Task<ActionResult<Result<string>>> ApprovePendingStore(Guid storeId, CancellationToken cancellationToken)
    {
        return await HandleMediatorResult(await _mediator.Send(new ApprovePendingStoreCommand(storeId), cancellationToken));
    }

    [HttpPut("RejectPendingStore/{storeId}")]
    public async Task<ActionResult<Result<bool>>>RejectPendingStore(Guid storeId, CancellationToken cancellationToken)
    {
        return await HandleMediatorResult(await _mediator.Send(new RejectPendingStoreCommand(storeId), cancellationToken));
    }

    [HttpGet("GetPendingStores")]
    public async Task<ActionResult<PaginatedResult<GetPendingStoresQueryDto>>> GetPendingStores([FromBody] GetPendingStoresQuery query, CancellationToken cancellationToken)
    {
        return await HandleMediatorResult(await _mediator.Send(query, cancellationToken));
    }
}