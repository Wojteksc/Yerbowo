namespace Yerbowo.Integration.Tests.Web.Controllers;

public class ProductsControllerTest(WebApplicationFactory<Startup> factory) : ApiTestBase(factory)
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        var categorySeeder = new CategorySeeder(_scope);
        var subcategorySeeder = new SubcategorySeeder(_scope);
        var productSeeder = new ProductSeeder(_scope);

        var yerbaMateCategoryId = await categorySeeder.SeedAsync(
            name: "Yerba Mate",
            description: "Test category Yerba Mate",
            image: "yerba-mate.png");

        var classicSubcategoryId = await subcategorySeeder.SeedAsync(
            categoryId: yerbaMateCategoryId,
            name: "Klasyczne");

        await productSeeder.SeedAsync(
            code: "PRODUCT-PAGING-001",
            name: "Product Paging 1",
            price: 10m,
            subcategoryId: classicSubcategoryId);

        await productSeeder.SeedAsync(
            code: "PRODUCT-PAGING-002",
            name: "Product Paging 2",
            price: 20m,
            subcategoryId: classicSubcategoryId);

        await productSeeder.SeedAsync(
            code: "PRODUCT-PAGING-003",
            name: "Product Paging 3",
            price: 30m,
            subcategoryId: classicSubcategoryId);
    }

    [Fact]
    public async Task GetProductsByPaging_Should_ReturnTheSameQuantity()
    {
        const int quantity = 3;

        var products = await _httpClient.GetAsync<List<ProductCardDto>>(
            $"api/products?category=yerba-mate&subcategory=klasyczne&pageNumber=1&pageSize={quantity}");

        products.Should().HaveCount(quantity);
    }
}