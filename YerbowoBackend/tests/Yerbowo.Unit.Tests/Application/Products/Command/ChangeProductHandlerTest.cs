namespace Yerbowo.Unit.Tests.Application.Products.Command;

public class ChangeProductHandlerTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly ChangeProductHandler _handler;

    private IStringLocalizer<SharedResource> _localizer;

    Guid ProductId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    Guid SubcategoryId = Guid.Parse("10000000-0000-0000-0000-000000000001");

    public ChangeProductHandlerTest()
    {
        _productRepositoryMock = new();

        _localizer = StringLocalizerFactory.Create();

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
            Id = ProductId,
            SubcategoryId = SubcategoryId,
            Code = "Code_new",
            Name = "Name_new",
            Description = "Description_new",
            Price = 20,
            Stock = 5,
            State = ProductState.Bestseller,
            Image = "Image_new.png",
        };

        var productDb = new Product(
            ProductId,
            SubcategoryId,
            "code",
            "name",
            "description",
            34,
            34,
            10,
            ProductState.None,
            "image.png");

        var expectedProduct = new Product(
            ProductId,
            SubcategoryId,
            request.Code,
            request.Name,
            request.Description,
            request.Price,
            34,
            request.Stock,
            request.State,
            request.Image);

        Product productUpdated = null;

        _productRepositoryMock
            .Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(productDb);
        
        _productRepositoryMock
            .Setup(x => x.ExistsAsync(request.Name.ToSlug()))
            .ReturnsAsync(false);
        
        _productRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Product>()))
            .Callback<Product>(p => productUpdated = p);

        await _handler.Handle(request, CancellationToken.None);

        _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
        productUpdated.Should().BeEquivalentTo(expectedProduct);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_ProductDoesNotExist(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.ProductNotFound];

        var request = new ChangeProductCommand { Id = ProductId, Name = "Name of the product" };

        _productRepositoryMock
            .Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync((Product)null);

        Func<Task> act = () => _handler.Handle(request, CancellationToken.None);
        var exception = await Assert.ThrowsAsync<ProductNotFoundException>(act);
        exception.Message.Should().Be(expectedMessage);
        _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_ProductNameAlreadyExists(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.ProductNameIsAlreadyExists];

        var request = new ChangeProductCommand { Id = ProductId, Name = "Name of the product" };

        var product = new Product(
            ProductId,
            SubcategoryId,
            "code",
            "Name of the product",
            "description",
            34,
            26,
            10,
            ProductState.Promotion,
            "image.png");

        _productRepositoryMock
           .Setup(x => x.GetAsync(request.Id))
           .ReturnsAsync(product);
        _productRepositoryMock
            .Setup(x => x.ExistsAsync(request.Name.ToSlug()))
            .ReturnsAsync(true);

        Func<Task> act = () => _handler.Handle(request, CancellationToken.None);
        var exception = await Assert.ThrowsAsync<ProductNameIsAlreadyExistsException>(act);
        exception.Message.Should().Be(expectedMessage);
        _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_TheNewPriceOfDiscountedProductIsHigher_Than_PriceOfCurrentProduct(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.PromotionalProductPriceMustBeLowerThanCurrent];

        var product = new Product(
            ProductId,
            SubcategoryId,
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
            Id = ProductId,
            SubcategoryId = SubcategoryId,
            Code = "Code_new",
            Name = "Name_new",
            Description = "Description_new",
            Price = 10000,
            State = ProductState.Promotion,
            Image = "Image_new.png",
        };

        _productRepositoryMock
           .Setup(x => x.GetAsync(request.Id))
           .ReturnsAsync(product);
        _productRepositoryMock
            .Setup(x => x.ExistsAsync(product.Name.ToSlug()))
            .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<PromotionalProductPriceMustBeLowerException>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }
}