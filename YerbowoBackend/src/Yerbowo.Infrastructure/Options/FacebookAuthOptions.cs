namespace Yerbowo.Infrastructure.Options;

public record FacebookAuthOptions
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}