using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.Authentication.Commands.ConfirmEmailOtpCommand;
using SnapSell.Application.Features.Authentication.Commands.Register;
using SnapSell.Application.Features.Authentication.Commands.SendConfirmationEmailOtp;
using SnapSell.Application.Features.Authentication.Queries.LogIn;
using SnapSell.Application.Features.Customer.Commands.AddCustomerInformation;
using SnapSell.Application.Features.store.Commands.CreateStore;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints;

public sealed class AccountController(ISender sender) : ApiControllerBase
{
    [HttpPost("Register")]
    public async Task<ActionResult<Result<RegisterResult>>> Register([FromBody] RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }

    [HttpPost("CreateStore")]
    public async Task<ActionResult<Result<CreateStoreResult>>> CreateStore([FromBody] CreateStoreCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }

    [HttpPut("AddCustomerInformation")]
    public async Task<ActionResult<Result<AddCustomerInfoResult>>> AddCustomerInformation(
        [FromBody] AddCustomerInformationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }

    [HttpPost("LogIn")]
    public async Task<ActionResult<Result<LogInResult>>> LogIn([FromForm] LogInQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }

    [HttpPost("SendEmailConfirmationOtp")]
    public async Task<ActionResult<Result<SendConfirmEmailOtpCommandDto>>> SendEmailConfirmationOtp(SendConfirmationEmailOtpCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }

    [HttpPost("ConfirmEmail")]
    public async Task<ActionResult<Result<ConfirmEmailOtpCommandResponse>>> ConfirmEmail(ConfirmEmailOtpCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResultAsync(result);
    }
}