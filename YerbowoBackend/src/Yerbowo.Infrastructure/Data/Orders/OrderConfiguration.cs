using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yerbowo.Domain.Orders;

namespace Yerbowo.Infrastructure.Data.Orders
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .IsRequired();

            builder.HasOne(o => o.Address);

            builder.HasOne(o => o.User)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(o => o.TotalCost)
                .HasColumnType("decimal(16,2)");
        }
    }
}
