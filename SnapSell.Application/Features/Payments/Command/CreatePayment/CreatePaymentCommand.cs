using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Application.Features.Payments.Command.CreatePayment
{
    public class CreatePaymentCommand:IRequest<Result<string>>
    {
    }
}
