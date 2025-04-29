using SnapSell.Domain.Models;

namespace SnapSell.Application.Interfaces.Authentication;

public interface IAuthenticationService
{
    public Task<string> GenerateTokenAsync(User user, bool isMobile = false);
    public Task<string> GenerateTokenAsync(User user, string role, bool isMobile = false);
}