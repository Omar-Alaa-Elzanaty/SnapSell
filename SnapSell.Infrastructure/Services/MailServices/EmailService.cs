using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.MailDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Infrastructure.Services.MailServices
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _filePath;
        private readonly IWebHostEnvironment _host;
        private readonly string _email;

        public EmailService(IConfiguration config, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
        {
            _emailSender = emailSender;
            _host = webHostEnvironment;
            _filePath = config.GetSection("Emails");
            _email = config.GetSection("Jwt:Audience").Value!;
        }

        public async Task<bool> SendEmailConfirmationOtp(string email, string otp)
        {
            var content = File.ReadAllText(_host.WebRootPath + _filePath["EmailConfirmation"]);

            return await _emailSender.SendMailUsingRazorTemplateAsync(new EmailRequestDto
            {
                Body = content,
                BodyData = new { OTP = otp },
                From = _email,
                Subject = "SnapSell Email Confirmation",
                To = email
            });
        }
    }
}
