using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yerbowo.Domain.Orders;

namespace Yerbowo.Infrastructure.Data.Orders
{
	public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.Property(oi => oi.Price)
				.HasColumnType("decimal(16,2)");
		}
	}
}
