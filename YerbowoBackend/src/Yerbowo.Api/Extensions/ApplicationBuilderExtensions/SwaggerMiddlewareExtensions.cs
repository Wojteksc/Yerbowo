namespace Yerbowo.Api.Extensions.ApplicationBuilderExtensions;

public static class SwaggerMiddlewareExtensions
{
    public static void UseSwaggers(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Yerbowo API v1");
        });
    }
}