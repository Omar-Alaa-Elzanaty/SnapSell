using SnapSell.Domain.Dtos.MailDtos;

namespace SnapSell.Application.Abstractions.Interfaces;

public interface IEmailSender
{
    Task<bool> SendMailUsingRazorTemplateAsync(EmailRequestDto request);
}