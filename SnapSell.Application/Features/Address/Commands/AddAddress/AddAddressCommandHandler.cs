using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
using System.Net;

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
        var client = await userManager.FindByIdAsync(request.ClinetId);

        if (client == null)
        {
            return Result<AddAddressResponse>.Failure(_localizer["UserNotFound"], HttpStatusCode.NotFound);
        }

        await unitOfWork.OrderAddressesRepo.AddAsync(request.Adapt<OrderAddress>());

        await unitOfWork.SaveAsync(cancellationToken);
        return Result<AddAddressResponse>.Success(_localizer["AddressSaved"]);

    }
}