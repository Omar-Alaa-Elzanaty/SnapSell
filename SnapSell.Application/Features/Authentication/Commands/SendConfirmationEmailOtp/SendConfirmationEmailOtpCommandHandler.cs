using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Authentication.Commands.SendConfirmationEmailOtp
{
    internal class SendConfirmationEmailOtpCommandHandler(
        IMemoryCache memoryCache,
        IStringLocalizer<SendConfirmationEmailOtpCommandHandler> localizer,
        IEmailService emailService,
        IWebHostEnvironment webHost) : IRequestHandler<SendConfirmationEmailOtpCommand, Result<SendConfirmEmailOtpCommandDto>>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly IEmailService _emailService = emailService;
        private readonly IWebHostEnvironment _webHost = webHost;
        private readonly IStringLocalizer<SendConfirmationEmailOtpCommandHandler> _localizer = localizer;

        public async Task<Result<SendConfirmEmailOtpCommandDto>> Handle(SendConfirmationEmailOtpCommand command, CancellationToken cancellationToken)
        {
            _memoryCache.Remove("ConfirmEmail" + command.Email);

            var otp = new Random().Next(10000, 99999).ToString();


            if (!await _emailService.SendEmailConfirmationOtp(command.Email, otp))
            {
                return Result<SendConfirmEmailOtpCommandDto>.Failure(_localizer["EmailOtpFailed"]);
            }

            _memoryCache.Set("ConfirmEmail" + command.Email, otp, TimeSpan.FromMinutes(5));

            if (_webHost.IsDevelopment())
            {
                return Result<SendConfirmEmailOtpCommandDto>.Success(new SendConfirmEmailOtpCommandDto(otp), _localizer["EmailOtpSent"]);
            }
            else
            {
                return Result<SendConfirmEmailOtpCommandDto>.Success(_localizer["EmailOtpSent"]);
            }
        }
    }
}
