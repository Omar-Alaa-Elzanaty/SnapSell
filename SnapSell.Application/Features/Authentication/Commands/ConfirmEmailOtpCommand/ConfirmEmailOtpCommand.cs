using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.Authentication.Commands.ConfirmEmailOtpCommand
{
    public record ConfirmEmailOtpCommand(
        string Email,
        string Otp) : IRequest<Result<ConfirmEmailOtpCommandResponse>>;

    public class ConfirmEmailOtpCommandResponse
    {
        public ConfirmOtpStoreInfoDto? Store { get; set; }
        public List<string>? Roles { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
    }

    public class ConfirmOtpStoreInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinimumDeliverPeriod { get; set; }
        public int MaximumDeliverPeriod { get; set; }
        public StoreStatusTypes Status { get; set; }
        public virtual DeliverPeriodTypes DeliverPeriodTypes { get; set; }
        public string? LogoUrl { get; set; }
    }
}
