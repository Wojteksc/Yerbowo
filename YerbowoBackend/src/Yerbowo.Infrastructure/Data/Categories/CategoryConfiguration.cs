using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yerbowo.Domain.Categories;

namespace Yerbowo.Infrastructure.Data.Categories
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasMany(c => c.Subcategories)
                .WithOne(s => s.Category)
                .IsRequired();
        }
    }
}
