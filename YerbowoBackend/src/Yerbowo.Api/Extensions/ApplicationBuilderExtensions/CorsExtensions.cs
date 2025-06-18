namespace Yerbowo.Api.Extensions.ApplicationBuilderExtensions;

public static class CorsExtensions
{
    public static void UseCorsOptions(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseCors(builder => builder
           .WithOrigins(configuration.GetValue<string>("App:CorsOrigins").Split(";"))
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials());
    }
}