﻿using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SnapSell.Application.Abstractions.Interfaces.Authentication;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Infrastructure.Services.Authentication;

public sealed class AuthenticationService(
    IOptions<JwtSettings> jwtSettings,
    UserManager<Account> userManager,
    RoleManager<IdentityRole> roleManager) : IAuthenticationService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<string> GenerateTokenAsync(Account user, bool isMobile = true)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecureKey));
        var userRoles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("is_mobile", isMobile.ToString().ToLower()),
            new Claim("token_type", isMobile ? "mobile" : "web")
        };

        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var expiration = isMobile
            ? DateTime.Now.AddDays(_jwtSettings.MobileExpireInDays)
            : DateTime.Now.AddDays(_jwtSettings.ExpireInDays);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiration,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);
    }

    public async Task<string> GenerateTokenAsync(Account user, string role, bool isMobile = false)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }


        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecureKey));
        var userRoles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("is_mobile", isMobile.ToString().ToLower()),
            new Claim("token_type", isMobile ? "mobile" : "web")
        };

        claims.AddRange(userRoles.Select(r =>
            new Claim(ClaimTypes.Role, r)));

        var expiration = isMobile
            ? DateTime.Now.AddDays(_jwtSettings.MobileExpireInDays)
            : DateTime.Now.AddDays(_jwtSettings.ExpireInDays);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiration,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);
    }

    public async Task<bool> AddRoleToUser(string userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        if (!await roleManager.RoleExistsAsync(role))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
            if (!roleResult.Succeeded)
                return false;
        }

        var addRoleResult = await userManager.AddToRoleAsync(user, role);
        return addRoleResult.Succeeded;
    }
}