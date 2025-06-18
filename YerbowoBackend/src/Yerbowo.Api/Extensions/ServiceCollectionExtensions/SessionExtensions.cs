namespace Yerbowo.Api.Extensions.ServiceCollectionExtensions;

public static class SessionExtensions
{
    public static void AddSessionOptions(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddSession(options =>
        {
            options.Cookie.IsEssential = true;
            options.Cookie.Name = "Cart";
            options.IdleTimeout = TimeSpan.FromDays(5);
        });
    }
}