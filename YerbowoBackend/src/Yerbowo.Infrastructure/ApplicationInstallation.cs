using Yerbowo.Infrastructure.Context;
using Yerbowo.Infrastructure.Data.Addresses;
using Yerbowo.Infrastructure.Data.Orders;
using Yerbowo.Infrastructure.Data.Products;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Infrastructure;

public static class ApplicationInstallation
{
    public static void AddYerbowoInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<YerbowoContextSeed>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
    }
}