using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.Authentication.Commands.ConfirmEmailOtpCommand
{
    internal class ConfirmEmailOtpCommandHandler : IRequestHandler<ConfirmEmailOtpCommand, Result<ConfirmEmailOtpCommandResponse>>
    {
        private readonly UserManager<Account> _userManager;
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ConfirmEmailOtpCommandHandler> _localizer;

        public ConfirmEmailOtpCommandHandler(
            UserManager<Account> userManager,
            IMemoryCache memoryCache,
            IUnitOfWork unitOfWork,
            IStringLocalizer<ConfirmEmailOtpCommandHandler> localizer)
        {
            _userManager = userManager;
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<ConfirmEmailOtpCommandResponse>> Handle(ConfirmEmailOtpCommand request, CancellationToken cancellationToken)
        {
            var otp = _memoryCache.Get<string>("ConfirmEmail" + request.Email);

            if (otp is null || otp != request.Otp)
            {
                return Result<ConfirmEmailOtpCommandResponse>.Failure(_localizer["InvalidOtp"]);
            }

            _memoryCache.Remove("ConfirmEmail" + request.Email);

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return Result<ConfirmEmailOtpCommandResponse>.Success(new ConfirmEmailOtpCommandResponse());
            }

            var roles = await _userManager.GetRolesAsync(user);

            var store = await _unitOfWork.StoresRepo.Entities
                .Where(x => x.SellerId.ToString() == user.Id)
                .ProjectToType<ConfirmOtpStoreInfoDto>()
                .FirstOrDefaultAsync(cancellationToken);

            return Result<ConfirmEmailOtpCommandResponse>.Success(new ConfirmEmailOtpCommandResponse()
            {
                FullName = user.FullName,
                UserName = user.UserName!,
                Roles = roles.ToList(),
                Store = store
            });
        }
    }
}