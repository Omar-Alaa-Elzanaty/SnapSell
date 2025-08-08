using DnsClient.Protocol;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Security.Claims;

namespace SnapSell.Application.Features.Address.Commands.SetAddressDefault
{
    internal class SetAddressDefaultCommandHandler : IRequestHandler<SetAddressDefaultCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;

        public SetAddressDefaultCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<Result<bool>> Handle(SetAddressDefaultCommand command, CancellationToken cancellationToken)
        {
            var clientId = _context.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            await _unitOfWork.OrderAddressesRepo.Entities
                .Where(x => x.AccountId == clientId && x.IsDefault == true)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.IsDefault, false), cancellationToken);

            await _unitOfWork.OrderAddressesRepo.Entities
                .Where(x => command.AddressId == x.Id)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.IsDefault, true), cancellationToken);

            return Result<bool>.Success();
        }
    }
}
