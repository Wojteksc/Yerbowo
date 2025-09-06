namespace Yerbowo.Infrastructure.Options;

public static class OptionsServiceRegistration
{
    public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppOptions>(configuration.GetSection("App"));
        services.AddSingleton<IAppSettings>(sp => sp.GetRequiredService<IOptions<AppOptions>>().Value);
    }
}