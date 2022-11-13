namespace Yerbowo.Functional.Tests.Web.Controllers;

public class CartControllerTest : ApiTestBase
{
	private readonly HttpClient _httpClient;

	public CartControllerTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
        _httpClient = CreateClient();
    }

	[Fact]
	public async Task AddProduct_Should_ReturnStatusCodeOk()
	{
		int productId = 1;
		
		var response = await PostAsync(productId, quantity: 1);

		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task AddTwoProducts_And_RemoveOneProduct_Should_ReturnOneProduct()
	{
		int firstProduct = 3;
		await PostAsync(firstProduct, quantity: 1);

		int secondProduct = 4;
		await PostAsync(secondProduct, quantity: 3);

		await DeleteAsync(firstProduct);

		var cart = await GetAsync();

		cart.Items.Should().ContainSingle();
	}

	[Fact]
	public async Task AddThreeTheSameProducts_Should_ReturnThreeQuantity()
	{
		int productId = 5;
		int quantity = 1;
		int total = 3;
		await PostAsync(productId, quantity);
		await PostAsync(productId, quantity);
		await PostAsync(productId, quantity);

		var cart = await GetAsync();

		cart.Items[0].Quantity.Should().Be(total);
	}

	[Fact]
	public async Task AddTheSameThreeProducts_And_DeleteThem_Should_ReturnEmptyCart()
	{
		int productId = 6;
		int quantity = 1;
		await PostAsync(productId, quantity);
		await PostAsync(productId, quantity);
		await PostAsync(productId, quantity);

		await DeleteAsync(productId);

		var cart = await GetAsync();

		cart.Items.Should().BeEmpty();
	}

	[Fact]
	public async Task AddTheSameProductManyTimesWithDifferentQuantities_ShouldReturnCorrectQuantity()
	{
		int productId = 7;
		int firstQuantity = 3;
		int secondQuantity = 2;
		int thirdQuantity = 5;
		int totalQuantity = firstQuantity + secondQuantity + thirdQuantity;
		await PostAsync(productId, firstQuantity);
		await PostAsync(productId, secondQuantity);
		await PostAsync(productId, thirdQuantity);

		var cart = await GetAsync();

		cart.Items[0].Quantity.Should().Be(totalQuantity);
	}

	[Fact]
	public async Task UpdateProduct_ShouldWorkCorrectly()
	{
		int productId = 8;
		int expectedQuantity = 4;
		await PostAsync(productId, 3);
		await PutAsync(productId, expectedQuantity);

		var cart = await GetAsync();

		cart.Items[0].Quantity.Should().Be(expectedQuantity);
	}

	[Fact]
	public async Task RemoveProduct_Should_ReturnStatusCodeOk()
	{
		int productId = 2;

		await PostAsync(productId, quantity: 1);
		var responseDelete = await DeleteAsync(productId);

		responseDelete.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task OverStock_Should_ThrowError()
	{
		Func<Task> func = async () => await PostAsync(productId: 9, quantity: 9999999);
		await Assert.ThrowsAsync<Exception>(func);
	}

	private async Task<CartDto> GetAsync()
	{
		return await _httpClient.GetAsync<CartDto>("/api/cart");
	}

	private async Task<HttpResponseMessage> PutAsync(int productId, int quantity)
	{
		return await _httpClient.PutAsync($"api/cart/{productId}", new ChangeCartItemCommand(productId, quantity));
	}

	private async Task<HttpResponseMessage> PostAsync(int productId, int quantity)
	{
		return await _httpClient.PostAsync($"api/cart", new AddCartItemCommand(productId, quantity));
	}

	private async Task<HttpResponseMessage> DeleteAsync(int productId)
	{
		return await _httpClient.DeleteAsync($"/api/cart/{productId}");
	}
}