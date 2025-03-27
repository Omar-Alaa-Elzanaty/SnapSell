using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Domain.Dtos.PaymobDtos
{
    public class PaymobCreatePaymentResponseDto
    {
        public string ClientSecretKey { get; set; }
        public string RedirectUrl { get; set; }
    }
}
