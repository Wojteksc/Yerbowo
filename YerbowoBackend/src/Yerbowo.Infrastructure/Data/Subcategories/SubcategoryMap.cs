using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yerbowo.Domain.Subcategories;

namespace Yerbowo.Infrastructure.Data.Subcategories
{
    public class SubcategoryMap : IEntityTypeConfiguration<Subcategory>
    {
        public void Configure(EntityTypeBuilder<Subcategory> builder)
        {
            builder.HasMany(s => s.Products)
                .WithOne(p => p.Subcategory)
                .IsRequired();
        }
    }
}
