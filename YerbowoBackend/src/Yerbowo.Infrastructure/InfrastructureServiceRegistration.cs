using Yerbowo.Infrastructure.Ids;

namespace Yerbowo.Infrastructure;

[ExcludeFromCodeCoverage]
public static class InfrastructureServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IInterfaceConverterJsonOptions, InterfaceConverterJsonOptions>();
        services.AddSingleton<IAssemblyExecutor, AssemblyExecutor>();
        services.AddScoped<IRequestDispatcher, RequestDispatcher>();

        services.AddOptions(configuration);
        services.AddMiddlewares();
        services.AddEmailServices(configuration);
        services.AddDalServices(configuration);
        services.AddAuthenticationServices(configuration);

        services.AddSingleton<IIdGenerator, SequentialGuidGenerator>();
        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddSingleton<IPasswordManager, PasswordManager>();
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        return app.UseExceptionMiddleware();
    }
}