namespace Yerbowo.Integration.Tests.Helpers;

public static class DbContextHelper
{
    public static YerbowoContext GetInMemory()
    {
        var options = new DbContextOptionsBuilder<YerbowoContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        // don't raise the error warning us that the in memory db doesn't support transactions
        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        .Options;
        var context = new YerbowoContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }
}