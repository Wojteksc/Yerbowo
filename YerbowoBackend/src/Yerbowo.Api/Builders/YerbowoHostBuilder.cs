namespace Yerbowo.Api.Builders;

public class YerbowoHostBuilder
{
    public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration config) =>
        Host.CreateDefaultBuilder(args)
        .UseSerilog() //Uses Serilog instead of default .NET Logger
        .ConfigureAppConfiguration(builder => builder.AddConfiguration(config))
        .ConfigureAppConfiguration((context, config) =>
        {
            Log.Information("ConfigureAppConfiguration Start");
            if (context.HostingEnvironment.IsDevelopment())
                return;

            Log.Information("AddAzureKeyVault Start");
            config.AddAzureKeyVault();
            Log.Information("AddAzureKeyVault END");
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            Log.Information("UseStartup<Startup>() Start");
            webBuilder.UseStartup<Startup>();
            Log.Information("UseStartup<Startup>() END");
        });
}