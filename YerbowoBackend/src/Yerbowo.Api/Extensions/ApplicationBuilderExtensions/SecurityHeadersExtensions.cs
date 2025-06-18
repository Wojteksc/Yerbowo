namespace Yerbowo.Api.Extensions.ApplicationBuilderExtensions;

public static class SecurityHeadersExtensions
{
    public static void UseSecurityHeaders(this IApplicationBuilder app)
    {
        app.UseHsts(options => options.MaxAge(days: 365).IncludeSubdomains()); // Strict-Transport-Security: max-age=31536000; includeSubDomains
        app.UseXContentTypeOptions(); // X-Content-Type-Options: nosniff
        app.UseXfo(options => options.SameOrigin()); // X-Frame-Options: SameOrigin
        app.UseXXssProtection(options => options.EnabledWithBlockMode()); // X-XSS-Protection: 1; mode=block
        app.UseReferrerPolicy(options => options.StrictOriginWhenCrossOrigin()); // Referrer-Policy: strict-origin-when-cross-origin
    }
}