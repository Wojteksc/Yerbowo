namespace Yerbowo.Unit.Tests.Application.Products.Command;

public class RemoveProductHandlerTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly RemoveProductHandler _handler;
    private readonly Product _product;

    public RemoveProductHandlerTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _handler = new RemoveProductHandler(_productRepositoryMock.Object);

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

    [Fact]
    public async Task Should_ThrowException_WhenProductIsNull()
    {
        var request = new RemoveProductCommand(999);

        _productRepositoryMock.Setup(x => x.GetAsync(request.Id))
            .Returns(Task.FromResult<Product>(null));

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be("Produkt nie istnieje");
        _productRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task Should_ThrowException_WhenProductIsRemoved()
    {
        _product.IsRemoved = true;

        var request = new RemoveProductCommand(2);

        _productRepositoryMock.Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(_product);

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be("Produkt nie istnieje");
        _productRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<Product>()), Times.Never);
    }
}
