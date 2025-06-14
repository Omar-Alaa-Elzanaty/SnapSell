using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Authentication.Commands.SendConfirmationEmailOtp
{
    internal class SendConfirmationEmailOtpCommandHandler(
        IMemoryCache memoryCache,
        IStringLocalizer<SendConfirmationEmailOtpCommandHandler> localizer) : IRequestHandler<SendConfirmationEmailOtpCommand, Result<string>>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly IStringLocalizer<SendConfirmationEmailOtpCommandHandler> _localizer = localizer;

        public async Task<Result<string>> Handle(SendConfirmationEmailOtpCommand command, CancellationToken cancellationToken)
        {
            _memoryCache.Remove("ConfirmEmail" + command.Email);

            var otp = new Random().Next(10000, 99999).ToString();

            _memoryCache.Set("ConfirmEmail" + command.Email, otp, TimeSpan.FromMinutes(5));

            return Result<string>.Success(data: otp, _localizer["EmailOtpSent"]);
        }
    }
}
