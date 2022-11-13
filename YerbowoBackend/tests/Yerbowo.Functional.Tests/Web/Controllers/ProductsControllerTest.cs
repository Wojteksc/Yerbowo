namespace Yerbowo.Functional.Tests.Web.Controllers;

public class ProductsControllerTest : ApiTestBase
{
	private readonly HttpClient _httpClient;
	public ProductsControllerTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
        _httpClient = CreateClient();
    }

	[Fact]
	private async Task GetProductsByPaging_Shoould_ReturnTheSameQuantity()
	{
		const int Quantity = 3;
		var products = await _httpClient.GetAsync<List<ProductCardDto>>($"api/products?category=yerba-mate&subcategory=klasyczne&pageNumber=1&pageSize={Quantity}");

		Assert.Equal(products.Count, Quantity);
	}
}
