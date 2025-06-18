namespace Yerbowo.Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton(AutoMapperConfig.Initialize());

        services.AddScoped<IWebEncoder, WebEncoder>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
    }
}