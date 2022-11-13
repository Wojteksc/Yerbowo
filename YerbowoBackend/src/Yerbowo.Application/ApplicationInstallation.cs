namespace Yerbowo.Application;

public static class ApplicationInstallation
{
    public static void AddYerbowoApplication(this IServiceCollection services)
    {
        services.AddSingleton<IJwtHandler, JwtHandler>();
        services.AddSingleton<IPasswordValidator, PasswordValidator>();
        services.AddSingleton(AutoMapperConfig.Initialize());
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IVerificationEmailTemplateSender, VerificationEmailTemplateSender>();

        services.AddMediatR(Assembly.GetExecutingAssembly());
    }
}