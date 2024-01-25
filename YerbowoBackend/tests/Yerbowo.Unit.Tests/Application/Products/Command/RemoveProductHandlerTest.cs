namespace Yerbowo.Unit.Tests.Application.Products.Command;

public class RemoveProductHandlerTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly RemoveProductHandler _handler;
    private readonly Product _product;

    public RemoveProductHandlerTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _handler = new RemoveProductHandler(
            _productRepositoryMock.Object,
            StringLocalizerFactory.Create());

        _product = new Product(1,
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
        var request = new RemoveProductCommand(1);
        var addresses = new List<Product>();

        _productRepositoryMock.Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(_product);
        _productRepositoryMock.Setup(x => x.RemoveAsync(_product))
            .Callback<Product>(a => addresses.Add(a));

        await _handler.Handle(request, CancellationToken.None);

        _productRepositoryMock.Verify(x => x.RemoveAsync(_product), Times.Once());
        addresses.Should().AllBeEquivalentTo(_product);
    }

    [Theory]
    [InlineData("en-US", "The product does not exist")]
    [InlineData("pl-PL", "Produkt nie istnieje")]
    public async Task Should_ThrowException_WhenProductIsNull(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        var request = new RemoveProductCommand(999);

        _productRepositoryMock.Setup(x => x.GetAsync(request.Id))
            .Returns(Task.FromResult<Product>(null));

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _productRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<Product>()), Times.Never);
    }

    [Theory]
    [InlineData("en-US", "The product does not exist")]
    [InlineData("pl-PL", "Produkt nie istnieje")]
    public async Task Should_ThrowException_WhenProductIsRemoved(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        _product.IsRemoved = true;

        var request = new RemoveProductCommand(2);

        _productRepositoryMock.Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(_product);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _productRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<Product>()), Times.Never);
    }
}
