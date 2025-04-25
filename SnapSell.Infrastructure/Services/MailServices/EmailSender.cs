using FluentEmail.Core;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.MailDtos;

namespace SnapSell.Infrastructure.Services.MailServices
{
    public class EmailSender : IEmailSender
    {
        private readonly IFluentEmailFactory _fluentEmail;

        public EmailSender(IFluentEmailFactory factory)
        {
            _fluentEmail = factory;
        }

        public async Task<bool> SendMailUsingRazorTemplateAsync(EmailRequestDto request)
        {
            try
            {
                var sendResponse = await _fluentEmail
                                        .Create()
                                        .SetFrom(request.From, "PSolve")
                                        .To(request.To)
                                        .Subject(request.Subject)
                                        .UsingTemplate(request.Body, request.BodyData)
                                        .SendAsync();

                return sendResponse.Successful;
            }
            catch
            {
                return false;
            }
        }
    }
}
