using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Domain.Dtos
{
    public class PaymobIntenstionDto
    {
        public double Amount { get; set; } 
        public List<int>Payment_Methods { get; set; }
        public string Currency { get; set; }
        public PaymobBillingDto Billing_Data { get; set; }
    }

    public class PaymobBillingDto
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Phone_Number { get; set; }
    }
}
