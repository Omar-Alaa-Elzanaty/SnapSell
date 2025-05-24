using SnapSell.Domain.Models;

namespace SnapSell.Application.Interfaces.Authentication;

public interface IAuthenticationService
{
    public Task<string> GenerateTokenAsync(Account user, bool isMobile = false);
    public Task<string> GenerateTokenAsync(Account user, string role, bool isMobile = false);
}