namespace devshop.api.Features.Auths.JWT;

public sealed class JwtSettings
{
    public const string SectionName = "JwtSettings";

    public string SecretKey { get; init; } = null!;

    public int ExpiryMinute { get; init; } = 60;

    public string Issuer { get; init; } = null!;

    public string Audience { get; init; } = null!;
}