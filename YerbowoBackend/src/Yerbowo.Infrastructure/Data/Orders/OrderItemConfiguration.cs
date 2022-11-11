namespace Yerbowo.Infrastructure.Data.Orders;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
	public void Configure(EntityTypeBuilder<OrderItem> builder)
	{
		builder.Property(oi => oi.Price)
			.HasColumnType("decimal(16,2)");
	}
}