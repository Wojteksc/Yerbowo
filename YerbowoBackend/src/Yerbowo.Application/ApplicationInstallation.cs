namespace Yerbowo.Application;

public static class ApplicationInstallation
{
    public static void AddYerbowoApplication(this IServiceCollection services)
    {
        services.AddSingleton(AutoMapperConfig.Initialize());
        services.AddSingleton<IInterfaceConverterJsonOptions, InterfaceConverterJsonOptions>();
        services.AddSingleton<IAssemblyExecutor, AssemblyExecutor>();

        services.AddScoped<IPasswordValidator, PasswordValidator>();
        services.AddScoped<IWebEncoder, WebEncoder>();
        

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
    }
}