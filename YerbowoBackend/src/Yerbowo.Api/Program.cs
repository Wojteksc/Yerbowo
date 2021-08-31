using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Yerbowo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("azurekeyvault.json")
                .Build();

            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            try
            {
                Log.Information("Application Starting.");
                CreateHostBuilder(args, config).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration config) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog() //Uses Serilog instead of default .NET Logger
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
}
