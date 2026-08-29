namespace Yerbowo.Integration.Tests.Seeders;

public sealed class CategorySeeder(IServiceScopeFactory scopeFactory)
{
    public async Task<Guid> SeedAsync(
        string name = "Test Category",
        string? description = null,
        string image = "test-image.jpg")
    {
        await using var scope = scopeFactory.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<YerbowoContext>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var existingCategory = await context.Categories
            .SingleOrDefaultAsync(x => x.Name == name);

        if (existingCategory is not null)
            return existingCategory.Id;

        var category = new Category(
            id: Guid.NewGuid(),
            name: name,
            description: description ?? $"Test description for {name}",
            image: image);

        await unitOfWork.ExecuteAsync(() =>
        {
            context.Categories.Add(category);
            return Task.CompletedTask;
        });

        return category.Id;
    }
}