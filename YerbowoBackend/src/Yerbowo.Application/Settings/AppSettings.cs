namespace Yerbowo.Application.Settings;

public record AppSettings
{
    public required string BaseUrl { get; init; }
}