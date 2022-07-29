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
}