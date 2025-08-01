using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Linq;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.Payments.Command.TokenCallback
{
    internal class PaymentTokenCommandHandler : IRequestHandler<PaymentTokenCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PaymentTokenCommandHandler> _logger;

        public PaymentTokenCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<PaymentTokenCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(PaymentTokenCommand command, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.OrdersRepo.Entities
                .Where(o => o.PaymobOrderId == command.Obj.OrderId)
                .Select(x => x.Account)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                //TODO: log the error
                return Result<int>.Failure("User not found for the provided order ID.");
            }

            await _unitOfWork.UserPaymentCardsRepo.AddAsync(new UserPaymentCard()
            {
                Token = command.Obj.Token,
                UserId = user.Id,
                MaskedPan = command.Obj.MaskedPan,
                CardSubType = command.Obj.CardSubType
            });

            await _unitOfWork.SaveAsync(cancellationToken);

            return Result<int>.Success();
        }
    }
}
