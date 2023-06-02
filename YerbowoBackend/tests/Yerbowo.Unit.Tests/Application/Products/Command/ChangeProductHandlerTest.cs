using Yerbowo.Domain.Products;

namespace Yerbowo.Unit.Tests.Application.Products.Command;

public class ChangeProductHandlerTest
{
    private readonly ChangeProductHandler _handler;
    private readonly Mock<IProductRepository> _productRepositoryMock;

    public ChangeProductHandlerTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();

        _handler = new ChangeProductHandler(
            _productRepositoryMock.Object,
            AutoMapperConfig.Initialize());
    }

    [Fact]
    public async Task Should_UpdateProductCorrectly()
    {
        var request = new ChangeProductCommand()
        {
            Id = 1,
            SubcategoryId = 2,
            Code = "Code_new",
            Name = "Name_new",
            Description = "Description_new",
            Price = 20,
            State = ProductState.Bestseller,
            Image = "Image_new.png",
        };

        var product = new Product(1,
            "code",
            "name",
            "description",
            34,
            34,
            10,
            ProductState.None,
            "image.png");

        var products = new List<Product>();

        _productRepositoryMock.Setup(x => x.GetAsync(request.Id))
           .ReturnsAsync(product);
        _productRepositoryMock.Setup(x => x.ExistsAsync(request.Name.ToSlug()))
            .ReturnsAsync(false);
        _productRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Product>()))
            .Callback<Product>(p => products.Add(p));

        await _handler.Handle(request, CancellationToken.None);

        _productRepositoryMock.Verify(x => x.UpdateAsync(product), Times.Once);
        product.Should().BeEquivalentTo(request, options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task Should_ThrowException_When_ProductDoesNotExist()
    {
        var request = new ChangeProductCommand { Id = 1, Name = "Name of the product" };

        _productRepositoryMock.Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync((Product)null);

        Func<Task> act = () => _handler.Handle(request, CancellationToken.None);
        var exception = await Assert.ThrowsAsync<Exception>(act);
        exception.Message.Should().Be("Nie znaleziono produktu");
        _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task Should_ThrowException_When_ProductNameAlreadyExists()
    {
        var request = new ChangeProductCommand { Id = 1, Name = "Name of the product" };

        var product = new Product(1,
            "code",
            "Name of the product",
            "description",
            34,
            26,
            10,
            ProductState.Promotion,
            "image.png");

        _productRepositoryMock.Setup(x => x.GetAsync(request.Id))
           .ReturnsAsync(product);
        _productRepositoryMock.Setup(x => x.ExistsAsync(request.Name.ToSlug()))
            .ReturnsAsync(true);

        Func<Task> act = () => _handler.Handle(request, CancellationToken.None);
        var exception = await Assert.ThrowsAsync<Exception>(act);
        exception.Message.Should().Be($"Produkt o nazwie {request.Name} już istnieje. Zmień nazwę.");
        _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task Should_ThrowException_When_TheNewPriceOfDiscountedProductIsHigher_Than_PriceOfCurrentProduct()
    {
        var product = new Product(1,
            "code",
            "name",
            "description",
            34,
            26,
            10,
            ProductState.Promotion,
            "image.png");

        var request = new ChangeProductCommand()
        {
            Id = 1,
            SubcategoryId = 2,
            Code = "Code_new",
            Name = "Name_new",
            Description = "Description_new",
            Price = 10000,
            State = ProductState.Promotion,
            Image = "Image_new.png",
        };

        _productRepositoryMock.Setup(x => x.GetAsync(request.Id))
           .ReturnsAsync(product);
        _productRepositoryMock.Setup(x => x.ExistsAsync(product.Name.ToSlug()))
            .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be("Nowe cena produktu objętego promocją musi być mniejsza od aktualnej ceny produktu.");
        _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }
}