using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Yerbowo.Application.Products;
using Yerbowo.Functional.Tests.Web.Extensions;

namespace Yerbowo.Functional.Tests.Web.Controllers
{
    public class ProductsControllerTest : IClassFixture<WebTestFixture>
	{
		private readonly HttpClient _httpClient;
		public ProductsControllerTest(WebTestFixture factory)
		{
			_httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions());
		}

		[Fact]
		private async Task Get_Products_By_Paging_Shoould_Return_The_Same_Quantity()
		{
			const int Quantity = 3;
			var products = await _httpClient.GetAsync<List<ProductCardDto>>($"api/products?category=yerba-mate&subcategory=klasyczne&pageNumber=1&pageSize={Quantity}");

			Assert.Equal(products.Count, Quantity);
		}
	}
}
