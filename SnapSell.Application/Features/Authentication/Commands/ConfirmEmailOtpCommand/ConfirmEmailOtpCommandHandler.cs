using System.Net;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Abstractions.Interfaces.Authentication;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Application.Features.Authentication.Commands.ConfirmEmailOtpCommand
{
    internal sealed class
        ConfirmEmailOtpCommandHandler : IRequestHandler<ConfirmEmailOtpCommand, Result<ConfirmEmailOtpCommandResponse>>
    {
        private readonly UserManager<Account> _userManager;
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ConfirmEmailOtpCommandHandler> _localizer;
        private readonly IAuthenticationService _authServices;

        public ConfirmEmailOtpCommandHandler(
            UserManager<Account> userManager,
            IMemoryCache memoryCache,
            IUnitOfWork unitOfWork,
            IStringLocalizer<ConfirmEmailOtpCommandHandler> localizer,
            IAuthenticationService authServices)
        {
            _userManager = userManager;
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _authServices = authServices;
        }

        public async Task<Result<ConfirmEmailOtpCommandResponse>> Handle(ConfirmEmailOtpCommand request,
            CancellationToken cancellationToken)
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
                if (await _userManager.Users.AnyAsync(x => x.UserName == request.Email, cancellationToken))
                {
                    return Result<ConfirmEmailOtpCommandResponse>.Failure(_localizer["UserAlreadyExists"]);
                }

                user = new Account()
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = "User",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var identity = await _userManager.CreateAsync(user);
                if (!identity.Succeeded)
                {
                    return Result<ConfirmEmailOtpCommandResponse>.Failure(
                        message: "failed to create user.",
                        statusCode: HttpStatusCode.BadRequest);
                }

                return Result<ConfirmEmailOtpCommandResponse>.Success(new ConfirmEmailOtpCommandResponse()
                {
                    Token = await _authServices.GenerateTokenAsync(user)
                });
            }


            var roles = await _userManager.GetRolesAsync(user);

            var store = await _unitOfWork.StoresRepo.Entities
                .Where(x => x.AccountId == user.Id)
                .ProjectToType<ConfirmOtpStoreInfoDto>()
                .FirstOrDefaultAsync(cancellationToken);

            var userInfo = user.Adapt<ConfirmOtpUserInfoDto>();
            userInfo.Store = store;
            userInfo.Roles = [.. roles];
            userInfo.Id = user.Id;
            userInfo.FirstName = user.FirstName;
            userInfo.LastName = user.LastName;
            userInfo.UserName = user.UserName;

            return Result<ConfirmEmailOtpCommandResponse>.Success(new ConfirmEmailOtpCommandResponse()
            {
                UserInfo = userInfo,
                Token = await _authServices.GenerateTokenAsync(user)
            });
        }
    }
}