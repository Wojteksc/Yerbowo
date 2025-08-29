namespace Yerbowo.Api.Extensions.ConfigurationBuilderExtensions;

public static class AppConfigurationExtensions
{
    public static void AddAppConfigurationFiles(this IConfigurationBuilder config, HostBuilderContext context)
    {
        config
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json")
            .AddEnvironmentVariables();
    }
}