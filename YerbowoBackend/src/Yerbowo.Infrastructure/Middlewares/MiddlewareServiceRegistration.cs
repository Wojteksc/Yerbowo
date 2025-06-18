namespace Yerbowo.Infrastructure.Middlewares;

public static class MiddlewareServiceRegistration
{
    public static void AddMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<ExceptionMiddleware>();
    }

    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}