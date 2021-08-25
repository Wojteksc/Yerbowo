using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Yerbowo.Application.Products.GetPagedProducts;
using Yerbowo.Domain.Products;
using Yerbowo.Fakers;
using Yerbowo.Fakers.Extensions;
using Yerbowo.Infrastructure.Data.Products;
using Yerbowo.Infrastructure.Helpers;
using Yerbowo.Integration.Tests.Builders;
using Yerbowo.Integration.Tests.Helpers;

namespace Yerbowo.Integration.Tests.Repositories.ProductRepositoryTests
{
    public class ProductRepositoryTests
	{
		Product product1 { get; } = new ProductFaker().UsePrivateConstructor().Generate();
		Product product2 { get; } = new ProductFaker().UsePrivateConstructor().Generate();

		public ProductRepositoryTests()
		{
		}

		[Fact]
		public async Task Adding_A_Product_Should_Also_Return_The_Same_Product()
		{
			IProductRepository repository = new ProductRepository(DbContextHelper.GetInMemory());

			//ACT
			await repository.AddAsync(product1);
			await repository.AddAsync(product2);
			Product exisitingProudct = await repository.GetAsync(product1.Id);

			//ASSERT
			exisitingProudct.Should().BeEquivalentTo(product1);
		}

		[Fact]
		public async Task Adding_Two_Products_Should_Also_Return_Two_Products()
		{
			IProductRepository repository = new ProductRepository(DbContextHelper.GetInMemory());

			//ACT
			await repository.AddAsync(product1);
			await repository.AddAsync(product2);
			IEnumerable<Product> products = await repository.GetAllAsync();

			//ASSERT
			products.Should().NotBeEmpty().And.HaveCount(2);
		}

		[Fact]
		public async Task Browse_Async_method_Should_Work_Correctly()
		{
			const int ProductQuantity = 100;
			IProductRepository repository = new ProductRepository(DbContextHelper.GetInMemory());

			List<Product> generatedProducts = new ProductGenerator().Generate(ProductQuantity).ToList();
			foreach (var product in generatedProducts)
			{
				await repository.AddAsync(product);
			}
			Product firstProduct = generatedProducts.First();
			var @params = new PageProductQuery()
			{
				Subcategory = firstProduct.Subcategory.Slug,
				Category = firstProduct.Subcategory.Category.Slug,
				PageNumber = 2,
				PageSize = 30
			};

			//ACT
			PagedList<Product> productsFromRepo = await repository.BrowseAsync(@params.PageNumber,
				@params.PageSize, @params.Category, @params.Subcategory);

			//ASSERT
			productsFromRepo.Should().HaveCount(@params.PageSize);
			productsFromRepo.PageSize.Should().Be(@params.PageSize);
			productsFromRepo.PageNumber.Should().Be(@params.PageNumber);
			productsFromRepo.TotalCount.Should().Be(ProductQuantity);
		}

		[Fact]
		public async Task When_Updating_Product_Should_Be_Updated_Correctly()
		{
			IProductRepository repository = new ProductRepository(DbContextHelper.GetInMemory());
			string newCode = "9031101";

			//ACT
			await repository.AddAsync(product1);
			product1.SetCode(newCode);
			await repository.UpdateAsync(product1);
			var existstingProduct = await repository.GetAsync(product1.Id);

			//ASSERT
			existstingProduct.Should().NotBeNull();
			existstingProduct.Code.Should().Be(newCode);
		}


		[Fact]
		public async Task When_Deleting_Product_Should_Be_Deleted_Correctly()
		{
			IProductRepository repository = new ProductRepository(DbContextHelper.GetInMemory());

			//ACT
			await repository.AddAsync(product1);
			await repository.AddAsync(product2);
			await repository.RemoveAsync(product1);

			var existstingProduct = await repository.GetAsync(product2.Id);
			var removedProduct = await repository.GetAsync(product1.Id);

			//ASSERT
			removedProduct.Should().BeNull();

			existstingProduct.Should().BeEquivalentTo(product2);
		}

	}
}
