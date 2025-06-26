namespace SnapSell.Application.Abstractions.Interfaces;

public interface IEmailService
{
    public Task<bool> SendEmailConfirmationOtp(string email, string otp);
}