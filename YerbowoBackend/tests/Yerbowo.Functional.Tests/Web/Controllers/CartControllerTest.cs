using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Yerbowo.Application.Cart;
using Yerbowo.Application.Cart.AddCartItems;
using Yerbowo.Application.Cart.ChangeCartItems;
using Yerbowo.Functional.Tests.Web.Extensions;
using System.Linq;

namespace Yerbowo.Functional.Tests.Web.Controllers
{
	public class CartControllerTest : IClassFixture<WebTestFixture>
    {
		private readonly HttpClient _httpClient;

		public CartControllerTest(WebTestFixture factory)
		{
			_httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions());
		}

		[Fact]
		public async Task Add_Product_To_Cart_Should_Return_Status_Code_204()
		{
			int productId = 1;
			var response = await PostAsync(productId, quantity: 1);

			Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
		}

		[Fact]
		public async Task Remove_Product_From_Cart_Should_Return_Status_Code_204()
		{
			int productId = 2;
			var responsePost = await PostAsync(productId, quantity: 1);
			var responseDelete = await DeleteAsync(productId);

			Assert.Equal(HttpStatusCode.NoContent, responsePost.StatusCode);
			Assert.Equal(HttpStatusCode.NoContent, responseDelete.StatusCode);
		}

		[Fact]
		public async Task Add_Two_Products_To_Cart_And_Remove_One_Product_From_Cart_Should_Return_One_Product()
		{
			int firstProduct = 3;
			await PostAsync(firstProduct, quantity: 1);

			int secondProduct = 4;
			await PostAsync(secondProduct, quantity: 3);

			await DeleteAsync(firstProduct);

			var cart = await GetAsync();

			Assert.Single(cart.Items);
		}

		[Fact]
		public async Task Add_Three_The_Same_Products_Should_Return_Three_Quantity()
		{
			int productId = 5;
			int quantity = 1;
			int total = 3;
			await PostAsync(productId, quantity);
			await PostAsync(productId, quantity);
			await PostAsync(productId, quantity);

			var cart = await GetAsync();

			Assert.Equal(total, cart.Items[0].Quantity);
		}

		[Fact]
		public async Task Add_Three_The_Same_Products_And_Delete_It_Should_Return_Empty_Cart()
		{
			int productId = 6;
			int quantity = 1;
			await PostAsync(productId, quantity);
			await PostAsync(productId, quantity);
			await PostAsync(productId, quantity);

			await DeleteAsync(productId);

			var cart = await GetAsync();

			Assert.True(!cart.Items.Any());
		}

		[Fact]
		public async Task Add_Product_With_Custom_Quantity_Should_Return_The_Same_Quantity_Of_The_Product()
		{
			int productId = 7;
			int quantity = 5;
			await PostAsync(productId, quantity);

			var cart = await GetAsync();

			Assert.Equal(quantity, cart.Items[0].Quantity);
		}

		[Fact]
		public async Task Add_Product_Many_Times_With_Different_Quantities_Should_Return_Correct_Quantity()
		{
			int productId = 7;
			int firstQuantity = 4;
			int secondQuantity = 2;
			int thirdQuantity = 5;
			int totalQuantity = firstQuantity + secondQuantity + thirdQuantity;
			await PostAsync(productId, firstQuantity);
			await PostAsync(productId, secondQuantity);
			await PostAsync(productId, thirdQuantity);

			var cart = await GetAsync();

			Assert.Equal(totalQuantity, cart.Items[0].Quantity);
		}

		[Fact]
		public async Task Update_Cart_Product_Should_Work_Properly()
		{
			int productId = 8;
			int expectedQuantity = 4;
			await PostAsync(productId, 3);
			await PutAsync(productId, expectedQuantity);

			var cart = await GetAsync();

			Assert.Equal(expectedQuantity, cart.Items[0].Quantity);
		}

		[Fact]
		public async Task Over_Stock_Will_Result_In_An_Error()
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
}
