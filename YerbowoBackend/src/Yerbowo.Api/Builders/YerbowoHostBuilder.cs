namespace Yerbowo.Api.Builders;

public class YerbowoHostBuilder
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureAppConfiguration((context, config) =>
        {
            config.AddAppSettings(context);

            if (!context.HostingEnvironment.IsDevelopment())
            {
                config.AddAzureKeyVault();
            }
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
}