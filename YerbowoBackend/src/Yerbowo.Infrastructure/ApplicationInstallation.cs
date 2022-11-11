namespace Yerbowo.Infrastructure;

public static class ApplicationInstallation
{
    public static void AddYerbowoInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<YerbowoContextSeed>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();

        var useInMemoryDb = configuration.GetValue("UseInMemoryDatabase", false);
        if (!useInMemoryDb)
        {
            services.AddDbContextPool<YerbowoContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
        else
        {
            services.AddDbContextPool<YerbowoContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });
        }
    }
}