namespace Yerbowo.Infrastructure.Email.Options;

[ExcludeFromCodeCoverage]
public record SendGridOptions
{
    public string ApiKey { get; init; }
    public Dictionary<string, string> TemplateIds { get; init; }
}