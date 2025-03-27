using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnapSell.Domain.Dtos.PaymobDtos
{
    public class PaymobCreatePaymentRequestDto
    {
        public double Amount { get; set; }
        public string Currency { get; set; }
        public BillingDataDto BillingData { get; set; }
        public string NotificationUrl { get; set; }
        public string RedirectionUrl { get; set; }
    }

    public class BillingDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Country { get; set; }
        public string PhoneNumber { get; set; }
    }
}
