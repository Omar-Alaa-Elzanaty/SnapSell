using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.Authentication.Commands.ConfirmEmailOtpCommand;
using SnapSell.Application.Features.Authentication.Commands.RegisterCustomer;
using SnapSell.Application.Features.Authentication.Commands.RegisterSeller;
using SnapSell.Application.Features.Authentication.Commands.SendConfirmationEmailOtp;
using SnapSell.Application.Features.Authentication.Queries.CustomerLogIn;
using SnapSell.Application.Features.Authentication.Queries.SellerLogin;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints;

public sealed class AccountController(ISender sender) : ApiControllerBase
{
    [HttpPost("RegisterSeller")]
    public async Task<ActionResult<Result<RegisterSellerResult>>> RegisterSeller([FromForm] RegisterSellerCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResult(result);
    }

    [HttpPost("RegisterCustomer")]
    public async Task<ActionResult<Result<RegisterCustomerResult>>> RegisterCustomer([FromForm] RegisterCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResult(result);
    }

    [HttpPost("LogInSeller")]
    public async Task<ActionResult<Result<SellerLogInResult>>> LogInSeller([FromForm] SellerLoginQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        return await HandleMediatorResult(result);
    }

    [HttpPost("LogInCustomer")]
    public async Task<ActionResult<Result<CustomerLogInResult>>> LogInCustomer([FromForm] CustomerLogInQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        return await HandleMediatorResult(result);
    }

    [HttpPost("SendEmailConfirmationOtp")]
    public async Task<ActionResult<Result<string>>> SendEmailConfirmationOtp(SendConfirmationEmailOtpCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResult(result);
    }

    [HttpPost("ConfirmEmail")]
    public async Task<ActionResult<Result<ConfirmEmailOtpCommandResponse>>>ConfirmEmail(ConfirmEmailOtpCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResult(result);
    }
}