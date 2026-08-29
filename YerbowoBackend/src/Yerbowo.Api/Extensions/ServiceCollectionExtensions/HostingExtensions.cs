namespace Yerbowo.Api.Extensions.ServiceCollectionExtensions;

public static class HostingExtensions
{
    public static void AddHostOptions(this IServiceCollection services)
    {
        services.Configure<HostOptions>(options =>
        {
            options.ServicesStartConcurrently = false;
            options.ServicesStopConcurrently = false;
        });
    }
}