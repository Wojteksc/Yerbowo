namespace Yerbowo.Api.Builders;

public class ConfigBuilder
{
    public static IConfigurationBuilder CreateConfigBuilder() =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddJsonFile("appsettings.Cloud.json", optional: true);
}