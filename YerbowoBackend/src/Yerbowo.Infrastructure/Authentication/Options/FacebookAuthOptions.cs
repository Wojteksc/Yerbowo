namespace Yerbowo.Infrastructure.Authentication.Options;

[ExcludeFromCodeCoverage]
public record FacebookAuthOptions
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}