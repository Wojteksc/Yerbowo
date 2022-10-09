using Yerbowo.Application.Services.Jwt;
using Yerbowo.Application.Services.PasswordValidator;
using Yerbowo.Application.Services.SendGrid;
using Yerbowo.Application.Settings;

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