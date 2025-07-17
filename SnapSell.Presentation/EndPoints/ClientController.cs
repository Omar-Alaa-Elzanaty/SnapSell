using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.Payments.Queries.Checkout;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints;

[Authorize(Roles = "Customer")]
public sealed class ClientController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public ClientController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Checkout")]
    public async Task<ActionResult<Result<List<CheckoutQueryDto>>>> Checkout()
    {
        var result = await _mediator.Send(new CheckoutQuery());
        return Ok(await HandleMediatorResultAsync(result));
    }
}