using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Payments.Command.CreatePayment
{
    internal class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<string>>
    {
        public Task<Result<string>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
