namespace Yerbowo.Api.Extensions.ConfigurationBuilderExtensions;

public static class AzureKeyVaultExtensions
{
    public static void AddAzureKeyVault(this IConfigurationBuilder config)
    {
        //var buildConfiguration = config.Build();

        //string kvURL = Environment.GetEnvironmentVariable("KeyVaultConfig__KVUrl") ?? buildConfiguration["KeyVaultConfig:KVUrl"];
        //string tenantId = Environment.GetEnvironmentVariable("KeyVaultConfig__TenantId") ?? buildConfiguration["KeyVaultConfig:TenantId"];
        //string clientId = Environment.GetEnvironmentVariable("KeyVaultConfig__ClientId") ?? buildConfiguration["KeyVaultConfig:ClientId"];
        //string clientSecret = Environment.GetEnvironmentVariable("KeyVaultConfig__ClientSecret") ?? buildConfiguration["KeyVaultConfig:ClientSecret"];

        //if (string.IsNullOrEmpty(kvURL) 
        //    || string.IsNullOrEmpty(tenantId) 
        //    || string.IsNullOrEmpty(clientId) 
        //    || string.IsNullOrEmpty(clientSecret))
        //    throw new Exception("Azure Key Vault settings are not configured!");


        //var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        //var client = new SecretClient(new Uri(kvURL), credential);
        //config.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());

        var buildConfiguration = config.Build();
        string kvUrl = Environment.GetEnvironmentVariable("KVUrl") ?? buildConfiguration["KeyVaultConfig:KVUrl"];
        if (string.IsNullOrEmpty(kvUrl))
            throw new Exception("Azure Key Vault settings are not configured!");

        var client = new SecretClient(new Uri(kvUrl), new DefaultAzureCredential());
        config.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
    }
}