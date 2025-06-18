namespace Yerbowo.Api.Extensions.ApplicationBuilderExtensions;

public static class LocalizationExtensions
{
    public static void UseRequestLocalizations(this IApplicationBuilder app)
    {
        var supportedCultures = new[]
        {
            new CultureInfo("pl-PL"),
            new CultureInfo("en-US")
        };

        var localizationOptions = new RequestLocalizationOptions
        {
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
            DefaultRequestCulture = new RequestCulture("pl-PL"),
            ApplyCurrentCultureToResponseHeaders = true
        };

        app.UseRequestLocalization(localizationOptions);
    }
}