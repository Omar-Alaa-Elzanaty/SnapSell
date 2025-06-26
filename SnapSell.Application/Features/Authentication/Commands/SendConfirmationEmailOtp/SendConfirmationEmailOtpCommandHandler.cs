using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Authentication.Commands.SendConfirmationEmailOtp
{
    internal class SendConfirmationEmailOtpCommandHandler(
        IMemoryCache memoryCache,
        IStringLocalizer<SendConfirmationEmailOtpCommandHandler> localizer,
        IEmailService emailService) : IRequestHandler<SendConfirmationEmailOtpCommand, Result<string>>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly IEmailService _emailService= emailService;
        private readonly IStringLocalizer<SendConfirmationEmailOtpCommandHandler> _localizer = localizer;

        public async Task<Result<string>> Handle(SendConfirmationEmailOtpCommand command, CancellationToken cancellationToken)
        {
            _memoryCache.Remove("ConfirmEmail" + command.Email);

            var otp = new Random().Next(10000, 99999).ToString();

            if (!await _emailService.SendEmailConfirmationOtp(command.Email, otp))
            {
                return Result<string>.Failure(_localizer["EmailOtpFailed"]);
            }

            _memoryCache.Set("ConfirmEmail" + command.Email, otp, TimeSpan.FromMinutes(5));

            return Result<string>.Success(_localizer["EmailOtpSent"]);
        }
    }
}
