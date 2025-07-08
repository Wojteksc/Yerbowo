using Yerbowo.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var hostBuilder = CreateHostBuilder(args);

        var host = hostBuilder.Build();
        var configuration = host.Services.GetRequiredService<IConfiguration>();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        try
        {
            Log.Information("Application Starting.");
            await host.RunAsync();
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

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddAppConfigurationFiles(context);

                if (context.HostingEnvironment.IsProduction() || 
                    context.HostingEnvironment.EnvironmentName == "Test")
                {
                    config.AddAzureKeyVault();
                }
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
