using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Address.Commands.SetAddressDefault
{
    public record SetAddressDefaultCommand : IRequest<Result<bool>>
    {
        public Guid AddressId { get; set; }

        public SetAddressDefaultCommand(Guid addressId)
        {
            AddressId = addressId;
        }
    }
}
