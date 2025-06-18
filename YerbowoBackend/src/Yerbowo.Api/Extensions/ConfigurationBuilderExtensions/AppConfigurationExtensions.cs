namespace Yerbowo.Api.Extensions.ConfigurationBuilderExtensions;

public static class AppConfigurationExtensions
{
    public static void AddAppConfigurationFiles(this IConfigurationBuilder config, HostBuilderContext context)
    {
        config.AddJsonFile("appsettings.json");
        config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json");
    }
}