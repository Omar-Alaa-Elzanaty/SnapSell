using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Authentication.Commands.SendConfirmationEmailOtp
{
    public sealed record SendConfirmationEmailOtpCommand(
        string Email
        ) : IRequest<Result<SendConfirmEmailOtpCommandDto>>;

    public class SendConfirmEmailOtpCommandDto
    {
        public string Otp { get; set; }

        public SendConfirmEmailOtpCommandDto(string otp)
        {
            Otp = otp;
        }
    }
}
