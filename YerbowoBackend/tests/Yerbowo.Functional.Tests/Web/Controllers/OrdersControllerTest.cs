using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Yerbowo.Functional.Tests.Web.Controllers
{
	public class OrdersControllerTest : IClassFixture<WebTestFixture>
	{
		private readonly HttpClient _httpClient;
		public OrdersControllerTest(WebTestFixture factory)
		{
			_httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions());
		}

		[Fact]
		public async Task Should_ReturnUnauthorizedStatusCode_When_UserIsAnonymous()
		{
			var response = await _httpClient.GetAsync("api/users/1/orders");

			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}
	}
}
