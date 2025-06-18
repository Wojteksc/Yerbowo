namespace Yerbowo.Unit.Tests.Application.Products.Command;

public class CreateProductHandlerTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    
    private readonly CreateProductHandler _handler;
    private readonly CreateProductCommand _request;

    private readonly IStringLocalizer<SharedResource> _localizer;

    const int ProductId = 1000;

    public CreateProductHandlerTest()
    {
        _productRepositoryMock = new();

        _localizer = StringLocalizerFactory.Create();

        _handler = new CreateProductHandler(
            _productRepositoryMock.Object,
            AutoMapperConfig.Initialize(),
            StringLocalizerFactory.Create());

        _request = new CreateProductCommand
        {
            SubcategoryId = 1,
            Code = "Code",
            Name = "Name of the product",
            Description = "Description",
            Price = 36,
            Stock = 20,
            State = ProductState.New,
            Image = "Image.png"
        };
    }

    [Fact]
    public async Task Should_CreateProductCorrectly()
    {
        var expectedInsertedProduct = new Product(1,
            "Code",
            "Name of the product",
            "Description",
            36,
            0,
            20,
            ProductState.New,
            "Image.png");
        typeof(Product).GetProperty(nameof(Product.Id)).SetValue(expectedInsertedProduct, ProductId, null);


        var expectedResult = new ProductDto
        {
            Code = "Code",
            Name = "Name of the product",
            Slug = "name-of-the-product",
            Description = "Description",
            Price = 36,
            Stock = 20,
            State = ProductState.New,
            Image = "Image.png"
        };

        var products = new List<Product>();

        _productRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Product>()))
            .Callback<Product>(p =>
            {
                typeof(Product).GetProperty(nameof(Product.Id)).SetValue(p, ProductId, null);
                products.Add(p);
            });

        int productId = await _handler.Handle(_request, CancellationToken.None);

        _productRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once());
        products.Should().AllBeEquivalentTo(expectedInsertedProduct);
        productId.Should().Be(ProductId);

    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_ProductNameAlreadyExists(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.ProductNameIsAlreadyExists];

        _productRepositoryMock
            .Setup(x => x.ExistsAsync(_request.Name.ToSlug()))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<ProductNameIsAlreadyExistsException>(() => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _productRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Never);
    }
}