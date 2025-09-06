namespace Yerbowo.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class ConfigurationExtensions
{
    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class
    {
        var section = configuration.GetRequiredSection(sectionName);
        return section.Get<T>() ?? throw new InvalidOperationException($"Section {sectionName} not found.");
    }
}