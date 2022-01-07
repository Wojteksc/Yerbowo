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

		
		[Fact]
		public async Task Should_ReturnProduct()
		{
			IProductRepository repository = new ProductRepository(DbContextHelper.GetInMemory());

			await repository.AddAsync(product1);
			await repository.AddAsync(product2);
			Product exisitingProudct = await repository.GetAsync(product1.Id);

			exisitingProudct.Should().BeEquivalentTo(product1);
		}

		
		[Fact]
		public async Task Should_ReturnTwoProducts()
		{
			IProductRepository repository = new ProductRepository(DbContextHelper.GetInMemory());

			await repository.AddAsync(product1);
			await repository.AddAsync(product2);
			IEnumerable<Product> products = await repository.GetAllAsync();

			products.Should().NotBeEmpty().And.HaveCount(2);
		}

		[Fact]
		public async Task Should_BrowseProducts()
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

			PagedList<Product> productsFromRepo = await repository.BrowseAsync(@params.PageNumber,
				@params.PageSize, @params.Category, @params.Subcategory);

			productsFromRepo.Should().HaveCount(@params.PageSize);
			productsFromRepo.PageSize.Should().Be(@params.PageSize);
			productsFromRepo.PageNumber.Should().Be(@params.PageNumber);
			productsFromRepo.TotalCount.Should().Be(ProductQuantity);
		}

		[Fact]
		public async Task Should_UpdateProduct()
		{
			IProductRepository repository = new ProductRepository(DbContextHelper.GetInMemory());
			string newCode = "9031101";

			await repository.AddAsync(product1);
			product1.SetCode(newCode);
			await repository.UpdateAsync(product1);
			var existstingProduct = await repository.GetAsync(product1.Id);

			existstingProduct.Should().NotBeNull();
			existstingProduct.Code.Should().Be(newCode);
		}


		[Fact]
		public async Task Should_DeleteProduct()
		{
			IProductRepository repository = new ProductRepository(DbContextHelper.GetInMemory());

			await repository.AddAsync(product1);
			await repository.AddAsync(product2);
			await repository.RemoveAsync(product1);

			var existstingProduct = await repository.GetAsync(product2.Id);
			var removedProduct = await repository.GetAsync(product1.Id);

			removedProduct.Should().BeNull();
			existstingProduct.Should().BeEquivalentTo(product2);
		}
	}
}