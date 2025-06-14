using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Application.Interfaces
{
    public interface IEmailService
    {
        public Task<bool> SendEmailConfirmationOtp(string email, string otp);
    }
}
