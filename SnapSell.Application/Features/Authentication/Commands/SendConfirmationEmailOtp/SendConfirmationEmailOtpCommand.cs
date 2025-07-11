using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Authentication.Commands.SendConfirmationEmailOtp
{
    public sealed record SendConfirmationEmailOtpCommand(
        string Email
        ) : IRequest<Result<string>>;
}
