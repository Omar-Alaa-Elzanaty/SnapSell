using System.Net;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Application.Features.Address.Commands.AddAddress;

internal sealed class AddAddressCommandHandler(
    IUnitOfWork unitOfWork,
    UserManager<Account> userManager,
    IStringLocalizer<AddAddressCommandHandler> _localizer)
    : IRequestHandler<AddAddressCommand, Result<AddAddressResponse>>
{
    public async Task<Result<AddAddressResponse>> Handle(AddAddressCommand request,
        CancellationToken cancellationToken)
    {
       var client = await unitOfWork.AccountsRepo.FindOnCriteriaAsync(i=>i.Id==request.ClinetId);    

        if(client == null)
        {
            return new Result<AddAddressResponse>() {Message="No user fount",StatusCode =HttpStatusCode.NotFound };    
        }
        var newAddress = request.Adapt<OrderAddress>();
        client.Addresses.Add(newAddress);

        await unitOfWork.SaveAsync(cancellationToken);
        return new Result<AddAddressResponse>() {Message = "Address Added Successfully." };   

    }
}