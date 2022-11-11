namespace Yerbowo.Api.Builders;

public class HostBuilder
{
    public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration config) =>
        Host.CreateDefaultBuilder(args)
        .UseSerilog() //Uses Serilog instead of default .NET Logger
        .ConfigureAppConfiguration(builder => builder.AddConfiguration(config))
        .ConfigureAppConfiguration((context, config) =>
        {
            if (context.HostingEnvironment.IsDevelopment())
                return;

            config.AddAzureKeyVault();
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
}