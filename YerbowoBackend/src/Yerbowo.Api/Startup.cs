using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yerbowo.Api.Extensions;
using Yerbowo.Infrastructure.Context;

namespace Yerbowo.Api
{
	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureTestingServices(IServiceCollection services)
        {
            services.AddDbContext<YerbowoContext>(options =>
                options.UseInMemoryDatabase("DatabaseInMemoryForFunctionalTests"));
            
            ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersOptions();
            services.AddMemoryCache();
            services.AddConfigure(Configuration);
            services.AddContext(Configuration);
            services.AddServices();
            services.AddAuthentication(Configuration);
            services.AddAuthorization();
            services.AddCors();
            services.AddSessionOptions();
            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, YerbowoContextSeed dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseForwardedHeadersOptions();
                app.UseExceptionHandlers();
                app.UseHttpsRedirection();
            }

            app.UseCorsOptions();
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
}
