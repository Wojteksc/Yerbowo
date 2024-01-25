namespace Yerbowo.Infrastructure;

public static class ApplicationInstallation
{
    public static void AddYerbowoInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<YerbowoContextSeed>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<INewsletterRepository, NewsletterRepository>();

        services.AddScoped<IRegistrationConfirmationEmailSender, RegistrationConfirmationEmailSender>();
        services.AddScoped<INewsletterInvitationEmailSender, NewsletterInvitationEmailSender>();
        services.AddScoped<INewsletterEmailSender, NewsletterEmailSender>();

        services.AddSingleton<InsertOutboxMessagesInterceptor>();

        services.AddDbContextPool<YerbowoContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetService<InsertOutboxMessagesInterceptor>());
            
            if (configuration.GetValue("UseInMemoryDatabase", false))
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }
            else
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        });
    }
}