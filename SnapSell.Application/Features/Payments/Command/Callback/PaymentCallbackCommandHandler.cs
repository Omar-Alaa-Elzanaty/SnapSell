using MediatR;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Payments.Command.Callback
{
    internal class PaymentCallbackCommandHandler : IRequestHandler<PaymentCallbackCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentCallbackCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<int>> Handle(PaymentCallbackCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
