using Microsoft.Extensions.Hosting.Internal;

namespace Yerbowo.Api.Extensions;

public static class ConfigurationBuilderExtensions
{
    public static void AddAzureKeyVault(this IConfigurationBuilder config)
    {
        var buildConfiguration = config.Build();

        string kvURL = buildConfiguration["KeyVaultConfig:KVUrl"];
        string tenantId = buildConfiguration["KeyVaultConfig:TenantId"];
        string clientId = buildConfiguration["KeyVaultConfig:ClientId"];
        string clientSecret = buildConfiguration["KeyVaultConfig:ClientSecret"];

        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        var client = new SecretClient(new Uri(kvURL), credential);
        config.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
    }

    public static void AddAppSettings(this IConfigurationBuilder config, HostBuilderContext context)
    {
        bool isTesting = context.HostingEnvironment.IsEnvironment("Testing");

        config.AddJsonFile("appsettings.json");
        config.AddJsonFile("appsettings.Development.json", optional: true);
        config.AddJsonFile("appsettings.Cloud.json", optional: !isTesting);
    }
}