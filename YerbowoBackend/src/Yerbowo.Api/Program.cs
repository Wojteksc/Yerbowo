namespace Yerbowo.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var config = ConfigBuilder
            .CreateConfigBuilder()
            .Build();

        //Initialize Logger
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();

        try
        {
            Log.Information("Application Starting.");
            YerbowoHostBuilder.CreateHostBuilder(args, config).Build().Run();
            Log.Information("CreateHostBuilder END");
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