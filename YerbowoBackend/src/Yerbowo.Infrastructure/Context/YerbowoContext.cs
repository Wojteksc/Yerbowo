using DotNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yerbowo.Domain.Addresses;
using Yerbowo.Domain.Categories;
using Yerbowo.Domain.Orders;
using Yerbowo.Domain.Products;
using Yerbowo.Domain.SeedWork;
using Yerbowo.Domain.Subcategories;
using Yerbowo.Domain.Users;

namespace Yerbowo.Infrastructure.Context
{
    public class YerbowoContext : DbContext
    {

        public YerbowoContext(DbContextOptions<YerbowoContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        /// <param name="isCurrentDate">Option ensures save current of date for CreatedAt and UpdatedAt</param>
        /// <returns></returns>
        public override int SaveChanges(bool isCurrentDate = true)
        {
            if (isCurrentDate)
                AddTimestamps();

            return base.SaveChanges(isCurrentDate = true);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow;

                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedAt = now;
                }
                ((BaseEntity)entity.Entity).UpdatedAt = now;
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly();
        }

    }
}
