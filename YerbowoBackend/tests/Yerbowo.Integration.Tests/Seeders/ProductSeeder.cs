namespace Yerbowo.Integration.Tests.Seeders;

public class ProductSeeder(IServiceScopeFactory scopeFactory)
{
    public async Task<Guid> SeedAsync(
        string code = "TEST-PRODUCT",
        string name = "Test Product",
        string description = "Test Description",
        decimal price = 29.99m,
        int stock = 100,
        ProductState state = ProductState.None,
        Guid? subcategoryId = null)
    {
        await using var scope = scopeFactory.CreateAsyncScope();

        var productRepository = scope.ServiceProvider
            .GetRequiredService<IProductRepository>();

        var unitOfWork = scope.ServiceProvider
            .GetRequiredService<IUnitOfWork>();

        if (!subcategoryId.HasValue)
        {
            var subcategorySeeder =
                new SubcategorySeeder(scopeFactory);

            subcategoryId = await subcategorySeeder.SeedAsync(
                name: $"Subcategory for {name}");
        }

        var product = new Product(
            id: Guid.NewGuid(),
            subcategoryId: subcategoryId.Value,
            code: code + Guid.NewGuid().ToString("N")[..8],
            name: name,
            description: description,
            price: price,
            oldPrice: price,
            stock: stock,
            state: state,
            image: "test-image.jpg");

        await unitOfWork.ExecuteAsync(async () =>
        {
            await productRepository.AddAsync(product);
        });

        return product.Id;
    }

    public async Task<List<Guid>> SeedManyAsync(
        int count,
        string namePrefix = "Test Product")
    {
        var ids = new List<Guid>();

        for (int i = 0; i < count; i++)
        {
            ids.Add(await SeedAsync(
                code: $"TEST-{i:D3}",
                name: $"{namePrefix} {i + 1}",
                price: 10m + i));
        }

        return ids;
    }
}