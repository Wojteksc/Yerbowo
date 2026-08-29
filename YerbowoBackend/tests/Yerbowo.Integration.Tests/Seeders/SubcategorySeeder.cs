namespace Yerbowo.Integration.Tests.Seeders;

public sealed class SubcategorySeeder(IServiceScopeFactory scopeFactory)
{
    public async Task<Guid> SeedAsync(
        Guid? categoryId = null,
        string name = "Test Subcategory")
    {
        if (!categoryId.HasValue)
        {
            var categorySeeder = new CategorySeeder(scopeFactory);

            categoryId = await categorySeeder.SeedAsync(
                name: $"Category for {name}");
        }

        await using var scope = scopeFactory.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<YerbowoContext>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var existingSubcategory = await context.Subcategories
            .SingleOrDefaultAsync(x =>
                x.CategoryId == categoryId.Value &&
                x.Name == name);

        if (existingSubcategory is not null)
            return existingSubcategory.Id;

        var subcategory = new Subcategory(
            id: Guid.NewGuid(),
            categoryId: categoryId.Value,
            name: name,
            description: $"Test description for {name}",
            image: "test-image.jpg");

        await unitOfWork.ExecuteAsync(() =>
        {
            context.Subcategories.Add(subcategory);
            return Task.CompletedTask;
        });

        return subcategory.Id;
    }
}