using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Application.Abstractions.Interfaces.Authentication;

public interface IAuthenticationService
{
    public Task<string> GenerateTokenAsync(Account user, bool isMobile = false);
    public Task<string> GenerateTokenAsync(Account user, string role, bool isMobile = false);
    public Task<bool> AddRoleToUser(string userId, string role);
}