namespace Yerbowo.Api.Extensions.ConfigurationBuilderExtensions;

public static class AzureKeyVaultExtensions
{
    public static void AddAzureKeyVault(this IConfigurationBuilder config)
    {
        string kvURL = Environment.GetEnvironmentVariable("AZURE_KEYVAULT_URL");
        string tenantId = Environment.GetEnvironmentVariable("AZURE_TENANT_ID");
        string clientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID");
        string clientSecret = Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET");

        if (string.IsNullOrEmpty(kvURL)
            || string.IsNullOrEmpty(tenantId)
            || string.IsNullOrEmpty(clientId)
            || string.IsNullOrEmpty(clientSecret))
            throw new Exception("Azure Key Vault environment variables are not configured!");

        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        var client = new SecretClient(new Uri(kvURL), credential);
        config.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
    }
}