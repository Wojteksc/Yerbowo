namespace Yerbowo.Integration.Tests.Web.Controllers;

public class OrdersControllerTest(WebApplicationFactory<Startup> factory) : ApiTestBase(factory)
{
    [Fact]
	public async Task Should_ReturnUnauthorizedStatusCode_When_UserIsAnonymous()
	{
		Guid userId = Guid.Parse("99999999-9999-9999-9999-999999999999");
		var response = await _httpClient.GetAsync($"/api/users/{userId}/orders");

		Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
	}
}