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
        if (env.IsDevelopment() && env.IsEnvironment("Testing"))
        {
            Log.Information("UseDeveloperExceptionPage Start");
            app.UseDeveloperExceptionPage();
            Log.Information("UseDeveloperExceptionPage End");
            Log.Information("UseSwaggers Start");
            app.UseSwaggers();
            Log.Information("UseSwaggers End");
        }
        else
        {
            Log.Information("UseForwardedHeadersOptions Start");
            app.UseForwardedHeadersOptions();
            Log.Information("UseForwardedHeadersOptions End");
            Log.Information("UseHttpsRedirection Start");
            app.UseHttpsRedirection();
            Log.Information("UseHttpsRedirection End");
        }
        Log.Information("UseExceptionHandlers Start");
        app.UseExceptionHandlers();
        Log.Information("UseExceptionHandlers End");
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