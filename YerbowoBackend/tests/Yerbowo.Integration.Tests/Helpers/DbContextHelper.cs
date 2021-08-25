using Microsoft.EntityFrameworkCore;
using Yerbowo.Infrastructure.Context;

namespace Yerbowo.Integration.Tests.Helpers
{
    public static class DbContextHelper
    {
        public static YerbowoContext GetInMemory()
        {
            DbContextOptions<YerbowoContext> options;
            var builder = new DbContextOptionsBuilder<YerbowoContext>();
            builder.UseInMemoryDatabase("Yerbowo");
            options = builder.Options;
            YerbowoContext context = new YerbowoContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }
    }
}
