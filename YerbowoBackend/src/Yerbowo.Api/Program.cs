using Serilog;

namespace Yerbowo.Api;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom
            .Configuration(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build())
            .CreateLogger();

        try
        {
            Log.Information("Application Starting.");

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
                })
                .Build()
                .Run();
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
}