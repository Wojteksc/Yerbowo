namespace Yerbowo.Infrastructure.Authentication.Options;

[ExcludeFromCodeCoverage]
public record GoogleAuthOptions
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}