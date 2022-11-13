namespace Yerbowo.Functional.Tests.Web.Controllers;

public class OrdersControllerTest : ApiTestBase
{
	private readonly HttpClient _httpClient;
	public OrdersControllerTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
        _httpClient = CreateClient();
    }

	[Fact]
	public async Task Should_ReturnUnauthorizedStatusCode_When_UserIsAnonymous()
	{
		var response = await _httpClient.GetAsync("api/users/999999/orders");

		Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
	}
}