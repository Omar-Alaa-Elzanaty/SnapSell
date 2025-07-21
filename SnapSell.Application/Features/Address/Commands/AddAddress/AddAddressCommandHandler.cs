using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Features.Address.Commands.AddAddress;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
using System.Net;

namespace SnapSell.Application.Features.store.Commands.CreateStore;

internal sealed class AddAddressCommandHandler(
    IUnitOfWork unitOfWork,
    UserManager<Account> userManager,
    IStringLocalizer<AddAddressCommandHandler> _localizer)
    : IRequestHandler<AddAddressCommand, Result<AddAddressResponse>>
{
    public async Task<Result<AddAddressResponse>> Handle(AddAddressCommand request,
        CancellationToken cancellationToken)
    {
       var client = await unitOfWork.ClientsRepo.FindOnCriteriaAsync(i=>i.Id==request.ClinetId);    

        if(client == null)
        {
            return new Result<AddAddressResponse>() {Message="No user fount",StatusCode =HttpStatusCode.NotFound };    
        }
        var newAddress = request.Adapt<OrderAddress>();
        client.Addresses.Add(newAddress);

        await unitOfWork.SaveAsync();

        return new Result<AddAddressResponse>() {Message = "Address Added Successfully." };   

    }
}