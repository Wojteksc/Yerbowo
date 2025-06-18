namespace Yerbowo.Infrastructure.DAL.Repositories.Categories;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasMany(c => c.Subcategories)
            .WithOne(s => s.Category)
            .IsRequired();
    }
}