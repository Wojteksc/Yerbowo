namespace Yerbowo.Unit.Tests.Factories;

public static class StringLocalizerFactory
{
    public static StringLocalizer<SharedResource> Create()
    {
        var options = Options.Create(new LocalizationOptions());
        var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
        var localizer = new StringLocalizer<SharedResource>(factory);

        return localizer;
    }
}