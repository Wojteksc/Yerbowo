namespace Yerbowo.Api.Extensions.ServiceCollectionExtensions;

public static class AuthorizationExtensions
{
    public static void AddAuthorizationOptions(this IServiceCollection services)
    {
        services.AddAuthorization(x => x.AddPolicy("HasAdminRole", p => p.RequireRole("admin")));
    }
}