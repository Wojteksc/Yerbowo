namespace Yerbowo.Infrastructure.DAL;

[ExcludeFromCodeCoverage]
public static class DalServiceRegistration
{
    public static void AddDalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DatabaseInitializer>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<INewsletterRepository, NewsletterRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Decorate(typeof(IRequestHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
        services.Decorate(typeof(IRequestHandler<,>), typeof(UnitOfWorkCommandHandlerDecorator<,>));

        services.AddHostedService<DatabaseInitializerJob>();
        services.AddHostedService<OutboxMessagesJob>();
        services.AddSingleton<InsertOutboxMessagesInterceptor>();

        services.AddDbContextPool<YerbowoContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetService<InsertOutboxMessagesInterceptor>());

            if (configuration.GetValue("UseInMemoryDatabase", false))
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString())
                       .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            }
            else
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        });
    }
}