namespace Yerbowo.Integration.Tests.Web.Controllers;

public class CartControllerTest(WebApplicationFactory<Startup> factory) : ApiTestBase(factory)
{
    private TestProducts _products;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        var productSeeder = new ProductSeeder(_scope);

        _products = new TestProducts(
            AddProduct: await productSeeder.SeedAsync(
                code: "CART-TEST-001",
                name: "Cart Test Product 1",
                price: 29.99m),

            RemoveProduct: await productSeeder.SeedAsync(
                code: "CART-TEST-002",
                name: "Cart Test Product 2",
                price: 39.99m),

            MultipleQuantity: await productSeeder.SeedAsync(
                code: "CART-TEST-003",
                name: "Cart Test Product 3",
                price: 19.99m),

            DeleteAll: await productSeeder.SeedAsync(
                code: "CART-TEST-004",
                name: "Cart Test Product 4",
                price: 49.99m),

            DifferentQuantities: await productSeeder.SeedAsync(
                code: "CART-TEST-005",
                name: "Cart Test Product 5",
                price: 59.99m),

            UpdateProduct: await productSeeder.SeedAsync(
                code: "CART-TEST-006",
                name: "Cart Test Product 6",
                price: 69.99m),

            RemoveStatusCode: await productSeeder.SeedAsync(
                code: "CART-TEST-007",
                name: "Cart Test Product 7",
                price: 79.99m),

            OverStock: await productSeeder.SeedAsync(
                code: "CART-TEST-008",
                name: "Cart Test Product 8",
                price: 89.99m)
        );
    }

    [Fact]
    public async Task AddProduct_Should_ReturnStatusCodeOk()
    {
        var response = await PostAsync(_products.AddProduct, quantity: 1);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddTwoProducts_And_RemoveOneProduct_Should_ReturnOneProduct()
    {
        await PostAsync(_products.AddProduct, quantity: 1);
        await PostAsync(_products.RemoveProduct, quantity: 3);
        await DeleteAsync(_products.AddProduct);

        var cart = await GetAsync();

        cart.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task AddThreeTheSameProducts_Should_ReturnThreeQuantity()
    {
        int quantity = 1;
        int total = 3;

        await PostAsync(_products.MultipleQuantity, quantity);
        await PostAsync(_products.MultipleQuantity, quantity);
        await PostAsync(_products.MultipleQuantity, quantity);

        var cart = await GetAsync();

        cart.Items[0].Quantity.Should().Be(total);
    }

    [Fact]
    public async Task AddTheSameThreeProducts_And_DeleteThem_Should_ReturnEmptyCart()
    {
        int quantity = 1;

        await PostAsync(_products.DeleteAll, quantity);
        await PostAsync(_products.DeleteAll, quantity);
        await PostAsync(_products.DeleteAll, quantity);

        await DeleteAsync(_products.DeleteAll);

        var cart = await GetAsync();

        cart.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task AddTheSameProductManyTimesWithDifferentQuantities_ShouldReturnCorrectQuantity()
    {
        int firstQuantity = 3;
        int secondQuantity = 2;
        int thirdQuantity = 5;
        int totalQuantity = firstQuantity + secondQuantity + thirdQuantity;

        await PostAsync(_products.DifferentQuantities, firstQuantity);
        await PostAsync(_products.DifferentQuantities, secondQuantity);
        await PostAsync(_products.DifferentQuantities, thirdQuantity);

        var cart = await GetAsync();

        cart.Items[0].Quantity.Should().Be(totalQuantity);
    }

    [Fact]
    public async Task UpdateProduct_ShouldWorkCorrectly()
    {
        int expectedQuantity = 4;

        await PostAsync(_products.UpdateProduct, 3);
        await PutAsync(_products.UpdateProduct, expectedQuantity);

        var cart = await GetAsync();

        cart.Items[0].Quantity.Should().Be(expectedQuantity);
    }

    [Fact]
    public async Task RemoveProduct_Should_ReturnStatusCodeOk()
    {
        await PostAsync(_products.RemoveStatusCode, quantity: 1);

        var responseDelete = await DeleteAsync(_products.RemoveStatusCode);

        responseDelete.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task OverStock_Should_ThrowError()
    {
        Func<Task> func = async () =>
            await PostAsync(_products.OverStock, quantity: 9999999);

        await Assert.ThrowsAsync<Exception>(func);
    }

    private async Task<CartDto> GetAsync()
    {
        return await _httpClient.GetAsync<CartDto>("/api/cart");
    }

    private async Task<HttpResponseMessage> PutAsync(Guid productId, int quantity)
    {
        return await _httpClient.PutAsync(
            $"api/cart/{productId}",
            new ChangeCartItemCommand(productId, quantity));
    }

    private async Task<HttpResponseMessage> PostAsync(Guid productId, int quantity)
    {
        return await _httpClient.PostAsync(
            "api/cart",
            new AddCartItemCommand(productId, quantity));
    }

    private async Task<HttpResponseMessage> DeleteAsync(Guid productId)
    {
        return await _httpClient.DeleteAsync($"/api/cart/{productId}");
    }

    private sealed record TestProducts(
        Guid AddProduct,
        Guid RemoveProduct,
        Guid MultipleQuantity,
        Guid DeleteAll,
        Guid DifferentQuantities,
        Guid UpdateProduct,
        Guid RemoveStatusCode,
        Guid OverStock);
}