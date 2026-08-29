namespace Yerbowo.Unit.Tests.Application.Products.Command;

public class RemoveProductHandlerTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly RemoveProductHandler _handler;
    private readonly Product _product;

    private IStringLocalizer<SharedResource> _localizer;

    private Guid ProductId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private Guid SubcategoryId = Guid.Parse("10000000-0000-0000-0000-000000000001");

    public RemoveProductHandlerTest()
    {
        _productRepositoryMock = new();

        _localizer = StringLocalizerFactory.Create();

        _handler = new RemoveProductHandler(
            _productRepositoryMock.Object,
            StringLocalizerFactory.Create());

        _product = new Product(
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
    }

    [Fact]
    public async Task Should_RemoveProductCorrectly()
    {
        var request = new RemoveProductCommand(ProductId);
        var addresses = new List<Product>();

        _productRepositoryMock
            .Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(_product);
        _productRepositoryMock
            .Setup(x => x.RemoveAsync(_product))
            .Callback<Product>(a => addresses.Add(a));

        await _handler.Handle(request, CancellationToken.None);

        _productRepositoryMock.Verify(x => x.RemoveAsync(_product), Times.Once());
        addresses.Should().AllBeEquivalentTo(_product);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_WhenProductIsNull(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.ProductNotFound];

        var request = new RemoveProductCommand(Guid.Parse("99999999-9999-9999-9999-999999999999"));

        _productRepositoryMock
            .Setup(x => x.GetAsync(request.Id))
            .Returns(Task.FromResult<Product>(null));

        var exception = await Assert.ThrowsAsync<ProductNotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _productRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<Product>()), Times.Never);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_WhenProductIsRemoved(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.ProductNotFound];

        _product.IsRemoved = true;

        var request = new RemoveProductCommand(ProductId);

        _productRepositoryMock
            .Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(_product);

        var exception = await Assert.ThrowsAsync<ProductNotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _productRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<Product>()), Times.Never);
    }
}
