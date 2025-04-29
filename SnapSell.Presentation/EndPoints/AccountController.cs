using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.DTOs.Authentication;
using SnapSell.Application.Interfaces;
using MediatR;
using SnapSell.Application.Features.Authentication.Commands.RegisterSeller;
using SnapSell.Application.Features.Authentication.Commands.RegisterCustomer;

namespace SnapSell.Presentation.EndPoints;

public sealed class AccountController(ICacheService cacheService, ISender sender) : ApiControllerBase(cacheService)
{
    [HttpPost("RegisterSeller")]
    public async Task<IActionResult> RegisterSeller([FromForm] RegisterRequestSellerDto request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterSellerCommand(request.SellerName, request.ShopName, request.Password);
        var result = await sender.Send(command, cancellationToken);
        return HandleMediatorResult<RegisterSellerResult>(result);
    }

    [HttpPost("RegisterCustomer")]
    public async Task<IActionResult> RegisterCustomer([FromForm] RegisterRequestCustomerDto request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCustomerCommand(request.CustomerName, request.UserName, request.Password);
        var result = await sender.Send(command, cancellationToken);
        return HandleMediatorResult<RegisterCustomerResult>(result);
    }
}