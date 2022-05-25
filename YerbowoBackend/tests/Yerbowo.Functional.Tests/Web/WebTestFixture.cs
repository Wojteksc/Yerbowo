using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yerbowo.Api;
using Yerbowo.Api.Builders;
using Yerbowo.Infrastructure.Context;
using HostBuilder = Yerbowo.Api.Builders.HostBuilder;

namespace Yerbowo.Functional.Tests.Web
{
    public class WebTestFixture : WebApplicationFactory<Startup>
	{
		protected override IHostBuilder CreateHostBuilder()
		{
			var config = ConfigBuilder
				.CreateConfigBuilder()
				.Build();

			return HostBuilder.CreateHostBuilder(null, config);
		}
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.UseEnvironment("Testing"); //it calls ConfigureTestingServices method

			builder.ConfigureServices((IServiceCollection services) =>
			{
				services.AddEntityFrameworkInMemoryDatabase();

				var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

				services.AddDbContext<YerbowoContext>(options =>
				{
					options.UseInMemoryDatabase("DatabaseInMemoryForFunctionalTests");
					options.UseInternalServiceProvider(provider);
				});

				var sp = services.BuildServiceProvider();

				using (var scope = sp.CreateScope())
				{
					var scopedServices = scope.ServiceProvider;
					var db = scopedServices.GetRequiredService<YerbowoContext>();
					db.Database.EnsureCreated();
				}
			});
		}
	}
}