namespace Yerbowo.Infrastructure.Email.Options;

[ExcludeFromCodeCoverage]
public record MailgunOptions
{
    public string ApiKey { get; init; }
    public string ApiBaseUrl { get; init; }
    public string DomainPath { get; init; }
    public Dictionary<string, string> TemplateIds { get; init; }
}