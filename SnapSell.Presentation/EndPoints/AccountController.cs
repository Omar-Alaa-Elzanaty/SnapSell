﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.Authentication.Commands.ConfirmEmailOtpCommand;
using SnapSell.Application.Features.Authentication.Commands.Register;
using SnapSell.Application.Features.Authentication.Commands.SendConfirmationEmailOtp;
using SnapSell.Application.Features.Authentication.Queries.CustomerLogIn;
using SnapSell.Application.Features.Authentication.Queries.SellerLogin;
using SnapSell.Application.Features.Customer.Commands.AddCustomerInformation;
using SnapSell.Application.Features.store.Commands.CreateStore;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints;

public sealed class AccountController(ISender sender) : ApiControllerBase
{
    [HttpPost("Register")]
    public async Task<ActionResult<Result<RegisterResult>>> Register([FromForm] RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResult(result);
    }

    [HttpPost("CreateStore")]
    public async Task<ActionResult<Result<CreateStoreResponse>>> CreateStore([FromBody] CreateStoreCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResult(result);
    }

    [HttpPut("AddCustomerInformation")]
    public async Task<ActionResult<Result<AddCustomerInformationRespose>>> AddCustomerInformation(
        [FromBody] AddCustomerInformationCommand command,
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
    public async Task<ActionResult<Result<ConfirmEmailOtpCommandResponse>>> ConfirmEmail(ConfirmEmailOtpCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return await HandleMediatorResult(result);
    }
}