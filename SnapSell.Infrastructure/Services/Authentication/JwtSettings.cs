namespace SnapSell.Infrastructure.Services.Authentication;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string SecureKey { get; set; } = null!;
    public double ExpireInDays { get; set; }
    public double MobileExpireInDays { get; set; }
}