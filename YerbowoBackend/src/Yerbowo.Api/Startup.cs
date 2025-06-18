namespace Yerbowo.Api;

public class Startup(IConfiguration Configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSwaggerDocumentation();
        services.AddControllersOptions();
        services.AddMemoryCache();
        services.AddHostOptions();
        services.AddApplicationServices();
        services.AddInfrastructureServices(Configuration);
        services.AddAuthorization();
        services.AddCors();
        services.AddLocalization();
        services.AddSessionOptions();
        services.AddResponseCaching();
        services.AddHttpContextAccessor();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwaggers();
        }
        else
        {
            app.UseForwardedHeadersOptions();
            app.UseHttpsRedirection();
        }

        app.UseInfrastructure();
        app.UseRequestLocalizations();
        app.UseCorsOptions(Configuration);
        app.UseSecurityHeaders();
        app.UseStaticFiles();
        app.UseDefaultFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();
        app.UseCookiePolicy();
        app.UseEndpointsOptions();
    }
}