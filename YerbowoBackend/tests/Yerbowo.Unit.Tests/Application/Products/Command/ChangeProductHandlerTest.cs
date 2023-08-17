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
            AutoMapperConfig.Initialize(),
            StringLocalizerFactory.Create());
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

    [Theory]
    [InlineData("en-US", "Product not found")]
    [InlineData("pl-PL", "Nie znaleziono produktu")]
    public async Task Should_ThrowException_When_ProductDoesNotExist(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        var request = new ChangeProductCommand { Id = 1, Name = "Name of the product" };

        _productRepositoryMock.Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync((Product)null);

        Func<Task> act = () => _handler.Handle(request, CancellationToken.None);
        var exception = await Assert.ThrowsAsync<Exception>(act);
        exception.Message.Should().Be(expectedMessage);
        _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [Theory]
    [InlineData("en-US", "The product already exists with that name")]
    [InlineData("pl-PL", "Produkt o tej nazwie już istnieje")]
    public async Task Should_ThrowException_When_ProductNameAlreadyExists(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

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
        exception.Message.Should().Be(expectedMessage);
        _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [Theory]
    [InlineData("en-US", "The new price of the promotional product must be lower than the current price of the product")]
    [InlineData("pl-PL", "Nowe cena produktu objętego promocją musi być mniejsza od aktualnej ceny produktu")]
    public async Task Should_ThrowException_When_TheNewPriceOfDiscountedProductIsHigher_Than_PriceOfCurrentProduct(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

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
        exception.Message.Should().Be(expectedMessage);
        _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }
}