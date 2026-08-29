namespace Yerbowo.Integration.Tests.Infrastructure;

public static class TestHostFactory
{
    public static WebApplicationFactory<Startup> Create(
        WebApplicationFactory<Startup> baseFactory,
        string environment,
        Action<IConfigurationBuilder> configureAppConfiguration)
    {
        return baseFactory.WithWebHostBuilder(builder =>
        {
            builder
                .ConfigureTestServices(services =>
                {
                    RemoveHostedService<DatabaseInitializerJob>(services);
                    RemoveHostedService<OutboxMessagesJob>(services);
                })
                .UseEnvironment(environment)
                .ConfigureAppConfiguration(configureAppConfiguration);
        });
    }

    private static void RemoveHostedService<THostedService>(IServiceCollection services)
        where THostedService : class, IHostedService
    {
        var descriptors = services
            .Where(descriptor =>
                descriptor.ServiceType == typeof(IHostedService) &&
                descriptor.ImplementationType == typeof(THostedService))
            .ToList();

        foreach (var descriptor in descriptors)
        {
            services.Remove(descriptor);
        }
    }
}