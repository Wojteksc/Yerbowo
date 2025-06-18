namespace Yerbowo.Infrastructure.Options;

public record GoogleAuthOptions
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}