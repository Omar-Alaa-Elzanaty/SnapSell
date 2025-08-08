using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.Address.Commands.AddAddress;
using SnapSell.Application.Features.Address.Commands.SetAddressDefault;
using SnapSell.Application.Features.Orders.Commands.Create;
using SnapSell.Application.Features.Payments.Queries.Checkout;
using SnapSell.Domain.Constants;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints;

[Authorize(Roles = Roles.Client)]
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

    [HttpPost("Order")]
    public async Task<ActionResult<Result<string>>> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(await HandleMediatorResultAsync(result));
    }

    [HttpPost("Address")]
    public async Task<ActionResult<AddAddressResponse>> CreateAddress([FromBody] AddAddressCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(await HandleMediatorResultAsync(result));
    }

    [HttpPut("Address/SetDefault/{id}")]
    public async Task<ActionResult<Result<bool>>> SetAddressAsDefault(Guid id)
    {
        var result = await _mediator.Send(new SetAddressDefaultCommand(id));

        return Ok(await HandleMediatorResultAsync(result));
    }
}