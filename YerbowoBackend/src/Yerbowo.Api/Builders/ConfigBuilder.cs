namespace Yerbowo.Api.Builders;

public class ConfigBuilder
{
    public static IConfigurationBuilder CreateConfigBuilder() =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("azurekeyvault.json");
}