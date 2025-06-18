namespace Yerbowo.Api.Extensions.ApplicationBuilderExtensions;

public static class ForwardedHeadersExtensions
{
    public static void UseForwardedHeadersOptions(this IApplicationBuilder app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
    }
}