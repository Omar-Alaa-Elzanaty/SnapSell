﻿using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Application.Applications.Payments.Commnad.Token
{
    internal class PaymentTokenCommandHandler : IRequestHandler<PaymentTokenCommand, Result<int>>
    {
        public Task<Result<int>> Handle(PaymentTokenCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
