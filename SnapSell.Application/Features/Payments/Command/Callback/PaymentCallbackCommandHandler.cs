using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.Payments.Command.Callback
{
    internal class PaymentCallbackCommandHandler : IRequestHandler<PaymentCallbackCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentCallbackCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(PaymentCallbackCommand command, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.OrdersRepo.Entities
                .FirstOrDefaultAsync(x => x.PaymobOrderId == command.Obj.Order.Id, cancellationToken);

            if(order is null)
            {
                return Result<int>.Failure("Order not found.");
            }

            if (command.Obj.Success)
            {
                var payment = order.Payment.FirstOrDefault(x => x.IntegrationId == command.Obj.IntegrationId);

                if(payment is null)
                {
                    return Result<int>.Failure("Payment not found for the order.");
                }

                payment.Status = PaymentStatus.Success;
                order.Status = OrderStatus.Approved;

                _unitOfWork.PaymentsRepo.Update(payment);
                _unitOfWork.OrdersRepo.Update(order);

                await _unitOfWork.SaveAsync(cancellationToken);
            }

            return Result<int>.Failure("need to write message for failure");
        }
    }
}
