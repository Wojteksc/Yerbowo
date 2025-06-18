namespace Yerbowo.Infrastructure.Options;

public class AppOptions : IAppSettings
{
    public required string BaseUrl { get; init; }
}