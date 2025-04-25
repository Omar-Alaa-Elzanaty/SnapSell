using SnapSell.Domain.Dtos.MailDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Application.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendMailUsingRazorTemplateAsync(EmailRequestDto request);
    }
}
