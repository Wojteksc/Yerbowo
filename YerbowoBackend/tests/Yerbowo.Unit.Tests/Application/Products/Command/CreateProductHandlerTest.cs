namespace Yerbowo.Unit.Tests.Application.Products.Command;

public class CreateProductHandlerTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly Mock<IIdGenerator> _idGeneratorMock = new();
    
    private readonly CreateProductHandler _handler;
    private readonly CreateProductCommand _request;

    private readonly IStringLocalizer<SharedResource> _localizer;

    Guid ProductId = Guid.Parse("00000000-0000-0000-0000-000000001000");
    Guid SubcategoryId = Guid.Parse("10000000-0000-0000-0000-000000001000");

    public CreateProductHandlerTest()
    {
        _localizer = StringLocalizerFactory.Create();

        _handler = new CreateProductHandler(
            _productRepositoryMock.Object,
            StringLocalizerFactory.Create(),
            _idGeneratorMock.Object);

        _request = new CreateProductCommand
        {
            SubcategoryId = SubcategoryId,
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
        var expectedInsertedProduct = new Product(
            ProductId,
            SubcategoryId,
            "Code",
            "Name of the product",
            "Description",
            36,
            0,
            20,
            ProductState.New,
            "Image.png");

        Product productDb = null;

        _productRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Product>()))
            .Callback<Product>(p =>
            {
                productDb = p;
            });

        _idGeneratorMock
            .Setup(x => x.Generate())
            .Returns(ProductId);

        Guid productId = await _handler.Handle(_request, CancellationToken.None);

        _productRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once());
        productDb.Should().BeEquivalentTo(expectedInsertedProduct);
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