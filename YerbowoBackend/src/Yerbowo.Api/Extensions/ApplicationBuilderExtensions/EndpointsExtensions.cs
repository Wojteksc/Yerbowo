namespace Yerbowo.Api.Extensions.ApplicationBuilderExtensions;

public static class EndpointsExtensions
{
    public static void UseEndpointsOptions(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapFallbackToController("Index", "Fallback");
        });
    }
}