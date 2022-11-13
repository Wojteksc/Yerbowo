namespace Yerbowo.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSwagger();
        services.AddControllersOptions();
        services.AddMemoryCache();
        services.AddSettings(Configuration);
        services.AddYerbowoInfrastructure(Configuration);
        services.AddYerbowoApplication();
        services.AddAuthentication(Configuration);
        services.AddAuthorization();
        services.AddCors();
        services.AddSessionOptions();
        services.AddResponseCaching();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, YerbowoContextSeed dbInitializer)
    {
        if (env.IsDevelopment() || env.IsEnvironment("Testing"))
        {
            app.UseDeveloperExceptionPage();
            app.UseSwaggers();
        }
        else
        {
            app.UseForwardedHeadersOptions();
            app.UseHttpsRedirection();
        }

        app.UseExceptionHandlers();
        app.UseCorsOptions(Configuration);
        app.UseRouting();
        app.UseSecurityHeaders();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseCookiePolicy();
        app.UseSession();
        app.UseEndpointsOptions();

        dbInitializer.Seed();
    }
}