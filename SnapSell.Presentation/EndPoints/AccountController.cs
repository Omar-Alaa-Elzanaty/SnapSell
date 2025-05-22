using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.Authentication.Commands.RegisterCustomer;
using SnapSell.Application.Features.Authentication.Commands.RegisterSeller;
using SnapSell.Application.Features.Authentication.Queries.CustomerLogIn;
using SnapSell.Application.Features.Authentication.Queries.SellerLogin;
using SnapSell.Application.Interfaces;

namespace SnapSell.Presentation.EndPoints;

public sealed class AccountController(ICacheService cacheService, ISender sender) : ApiControllerBase(cacheService)
{
    [HttpPost("RegisterSeller")]
    public async Task<IActionResult> RegisterSeller([FromForm] RegisterSellerCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleMediatorResult<RegisterSellerResult>(result);
    }

    [HttpPost("RegisterCustomer")]
    public async Task<IActionResult> RegisterCustomer([FromForm] RegisterCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleMediatorResult<RegisterCustomerResult>(result);
    }

    [HttpPost("LogInSeller")]
    public async Task<IActionResult> LogInSeller([FromForm] SellerLoginQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        return HandleMediatorResult<SellerLogInResult>(result);
    }

    [HttpPost("LogInCustomer")]
    public async Task<IActionResult> LogInCustomer([FromForm] CustomerLogInQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        return HandleMediatorResult<CustomerLogInResult>(result);
    }
}