namespace Yerbowo.Api.Builders;

public class ConfigBuilder
{
    public static IConfigurationBuilder CreateConfigBuilder() =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");
}