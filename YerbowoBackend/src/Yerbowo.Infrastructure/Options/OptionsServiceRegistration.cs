namespace Yerbowo.Infrastructure.Options;

public static class OptionsServiceRegistration
{
    public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<SendGridOptions>(configuration.GetSection("SendGrid"));
        services.Configure<AppOptions>(configuration.GetSection("App"));
        services.Configure<GoogleAuthOptions>(configuration.GetSection("Authentication:Google"));
        services.Configure<FacebookAuthOptions>(configuration.GetSection("Authentication:Facebook"));
        services.AddSingleton<IAppSettings>(sp => sp.GetRequiredService<IOptions<AppOptions>>().Value);
    }
}