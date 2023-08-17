namespace Yerbowo.Unit.Tests.Application.Cart.Command;

public class ChangeCartItemHandlerTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Product _productDb;
    private readonly CartProductItemDto _cartProductItem;

    const int _productId = 1000;
    public ChangeCartItemHandlerTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productDb = new Product(1, "code", "name", "description", 34, 34, 10, ProductState.None, "image.png");
        _cartProductItem = new CartProductItemDto
        {
            Id = _productId,
            Code = "code",
            Name = "name",
            Description = "description",
            Price = 34m,
            OldPrice = 34m,
            Stock = 10,
            State = ProductState.None,
            Image = "image.png",
            Slug = "name".ToSlug()
        };
        typeof(Product).GetProperty(nameof(Product.Id)).SetValue(_productDb, _productId, null);
    }

    [Fact]
    public async Task Should_ChangeProductQuantityInCart()
    {
        var request = new ChangeCartItemCommand(_productId, 10);

        var cartProducts = new List<CartItemDto>
        {
            new CartItemDto
            {
                Product = _cartProductItem,
                Quantity = 1,
            }
        };

        var expectedCartProducts = new List<CartItemDto>
        {
            new CartItemDto
            {
                Product = _cartProductItem,
                Quantity = 10,
            }
        };

        _productRepositoryMock.Setup(x => x.GetAsync(_productId))
            .ReturnsAsync(_productDb);
        var sessionMock = SessionMockHelper.SetupSession(Consts.CartSessionKey, cartProducts);
        var httpContextAccessor = HttpContextAccessorFactory.Create(sessionMock);
        var handler = CreateHandler(httpContextAccessor);

        var result = await handler.Handle(request, CancellationToken.None);

        result.Items.Should().BeEquivalentTo(expectedCartProducts);
        sessionMock.Verify(x => x.Set(Consts.CartSessionKey, It.IsAny<byte[]>()), Times.Once);
    }

    [Theory]
    [InlineData("en-US", "Product not found")]
    [InlineData("pl-PL", "Nie znaleziono produktu")]
    public async Task Should_ThrowException_When_ProductDoesNotExistsInCart(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        int productId = 999;
        var request = new ChangeCartItemCommand(productId, _productDb.Stock + 1);

        _productRepositoryMock.Setup(x => x.GetAsync(productId))
            .ReturnsAsync((Product)null);
        var sessionMock = SessionMockHelper.SetupSession();
        var httpContextAccessor = HttpContextAccessorFactory.Create(sessionMock);
        var handler = CreateHandler(httpContextAccessor);

        var exception = await Assert.ThrowsAsync<Exception>(
            () => handler.Handle(request, CancellationToken.None));

        exception.Message.Should().Be(expectedMessage);
        _productRepositoryMock.Verify(x => x.GetAsync(It.IsAny<int>()), Times.Never());
        sessionMock.Verify(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Never());
    }

    [Theory]
    [InlineData(0, "en-US", "Incorrect quantity")]
    [InlineData(-1, "pl-PL", "Nieprawidłowa ilość")]
    public async Task Should_ThrowException_When_RequestQuantityIsZeroOrNegative(int quantity, string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        int productId = 999;
        var request = new ChangeCartItemCommand(productId, quantity);
        var sessionMock = SessionMockHelper.SetupSession();
        var httpContextAccessor = HttpContextAccessorFactory.Create(sessionMock);
        var handler = CreateHandler(httpContextAccessor);

        var exception = await Assert.ThrowsAsync<Exception>(
            () => handler.Handle(request, CancellationToken.None));

        exception.Message.Should().Be(expectedMessage);
        _productRepositoryMock.Verify(x => x.GetAsync(It.IsAny<int>()), Times.Never());
        sessionMock.Verify(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Never());
    }

    [Theory]
    [InlineData("en-US", "Stock exceeded")]
    [InlineData("pl-PL", "Przekroczono zapas")]
    public async Task Should_ThrowException_When_ProductQuantityIsGreaterThanStock_And_ProductIsInCart(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        var request = new ChangeCartItemCommand(_productId, _productDb.Stock + 1);
        var cartProducts = new List<CartItemDto>
        {
            new CartItemDto
            {
                Product = _cartProductItem,
                Quantity = 1,
            }
        };

        _productRepositoryMock.Setup(x => x.GetAsync(_productId))
            .ReturnsAsync(_productDb);
        var sessionMock = SessionMockHelper.SetupSession(Consts.CartSessionKey, cartProducts);
        var httpContextAccessor = HttpContextAccessorFactory.Create(sessionMock);
        var handler = CreateHandler(httpContextAccessor);

        var exception = await Assert.ThrowsAsync<Exception>(
            () => handler.Handle(request, CancellationToken.None));

        exception.Message.Should().Be(expectedMessage);
        _productRepositoryMock.Verify(x => x.GetAsync(_productId), Times.Once);
        sessionMock.Verify(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Never());
    }

    private ChangeCartItemHandler CreateHandler(HttpContextAccessor httpContextAccessor)
    {
        return new ChangeCartItemHandler(
            httpContextAccessor,
            _productRepositoryMock.Object,
            AutoMapperConfig.Initialize(),
            StringLocalizerFactory.Create());
    }
}