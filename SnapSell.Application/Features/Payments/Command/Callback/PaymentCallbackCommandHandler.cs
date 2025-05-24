using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Application.Features.Payments.Command.Callback
{
    internal class PaymentCallbackCommandHandler : IRequestHandler<PaymentCallbackCommand, Result<int>>
    {
        public Task<Result<int>> Handle(PaymentCallbackCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
