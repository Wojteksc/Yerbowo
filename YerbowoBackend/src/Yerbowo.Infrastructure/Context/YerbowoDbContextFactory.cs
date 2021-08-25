using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Yerbowo.Infrastructure.Context
{
    public class YerbowoDbContextFactory : IDesignTimeDbContextFactory<YerbowoContext>
    {
        public YerbowoContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Yerbowo.Api"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            var optionsBuilder = new DbContextOptionsBuilder<YerbowoContext>();
            var connectionString = configuration.GetConnectionString("YerbowoDatabase");
            optionsBuilder.UseMySql(connectionString, b => b.MigrationsAssembly("Yerbowo.Infrastructure"));
            return new YerbowoContext(optionsBuilder.Options);
        }
    }
}
