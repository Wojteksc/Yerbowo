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
        Log.Information("AddSwagger Start");
        services.AddSwagger();
        Log.Information("AddSwagger EBD");

        services.AddControllersOptions();
        services.AddMemoryCache();
        services.AddSettings(Configuration);
        Log.Information("AddYerbowoInfrastructure Start");
        services.AddYerbowoInfrastructure(Configuration);
        Log.Information("AddYerbowoInfrastructure End");
        services.AddYerbowoApplication();
        services.AddAuthentication(Configuration);
        services.AddAuthorization();
        services.AddCors();
        services.AddSessionOptions();
        services.AddResponseCaching();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, YerbowoContextSeed dbInitializer)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwaggers();
        }
        else
        {
            Log.Information("UseForwardedHeadersOptions Start");
            app.UseForwardedHeadersOptions();
            Log.Information("UseForwardedHeadersOptions End");
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

        Log.Information("Seed Start");
        dbInitializer.Seed();
        Log.Information("Seed End");
    }
}