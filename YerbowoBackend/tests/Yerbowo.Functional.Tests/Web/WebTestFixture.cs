using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Yerbowo.Api;
using Yerbowo.Infrastructure.Context;

namespace Yerbowo.Functional.Tests.Web
{
	public class WebTestFixture : WebApplicationFactory<Startup>
	{
		protected override IHostBuilder CreateHostBuilder()
		{
			var config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.AddJsonFile("azurekeyvault.json")
			.Build();

			return Host.CreateDefaultBuilder()
			.ConfigureAppConfiguration(builder => builder.AddConfiguration(config))
			.ConfigureAppConfiguration((context, config) =>
			{
				var buildConfiguration = config.Build();

				string kvURL = buildConfiguration["KeyVaultConfig:KVUrl"];
				string tenantId = buildConfiguration["KeyVaultConfig:TenantId"];
				string clientId = buildConfiguration["KeyVaultConfig:ClientId"];
				string clientSecret = buildConfiguration["KeyVaultConfig:ClientSecret"];

				var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
				var client = new SecretClient(new Uri(kvURL), credential);
				config.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
			})
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			});
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
