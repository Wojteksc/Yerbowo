namespace Yerbowo.Application.Settings;

public record JwtSettings
{
    public required string Key { get; init; }

    public required string Issuer { get; init; }

    public required int ExpiryMinutes { get; init; }
}