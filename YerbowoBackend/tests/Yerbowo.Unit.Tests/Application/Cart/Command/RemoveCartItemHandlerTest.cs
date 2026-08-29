namespace Yerbowo.Unit.Tests.Application.Cart.Command;

public class RemoveCartItemHandlerTest
{
    private readonly CartProductItemDto _cartProductItem;

    Guid ProductId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    private IStringLocalizer<SharedResource> _localizer;

    public RemoveCartItemHandlerTest()
    {
        _localizer = StringLocalizerFactory.Create();

        _cartProductItem = new CartProductItemDto
        {
            Id = ProductId,
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
    }

    [Fact]
    public async Task Should_RemoveProductFromCart()
    {
        var request = new RemoveCartItemCommand(ProductId);

        var cartProducts = new List<CartItemDto>
        {
            new CartItemDto
            {
                Product = _cartProductItem,
                Quantity = 1,
            }
        };

        var expectedCartProducts = new List<CartItemDto>();

        var sessionMock = SessionMockHelper.SetupSession(SessionKeys.CartSession, cartProducts);
        var httpContextAccessor = HttpContextAccessorFactory.Create(sessionMock);
        var handler = CreateHandler(httpContextAccessor);

        var result = await handler.Handle(request, CancellationToken.None);

        result.Items.Should().BeEquivalentTo(expectedCartProducts);
        sessionMock.Verify(x => x.Set(SessionKeys.CartSession, It.IsAny<byte[]>()), Times.Once);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_ProductDoesNotExistInCart(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.ProductNotFound];

        Guid productId = Guid.Parse("99999999-9999-9999-9999-999999999999");
        var request = new RemoveCartItemCommand(productId);

        var cartProducts = new List<CartItemDto>
        {
            new CartItemDto
            {
                Product = _cartProductItem,
                Quantity = 1,
            }
        };

        var sessionMock = SessionMockHelper.SetupSession(SessionKeys.CartSession, cartProducts);
        var httpContextAccessor = HttpContextAccessorFactory.Create(sessionMock);
        var handler = CreateHandler(httpContextAccessor);

        var exception = await Assert.ThrowsAsync<ProductNotFoundException>(
            () => handler.Handle(request, CancellationToken.None));

        exception.Message.Should().Be(expectedMessage);
        sessionMock.Verify(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Never());
    }

    private RemoveCartItemHandler CreateHandler(HttpContextAccessor httpContextAccessor)
    {
        return new RemoveCartItemHandler(
            httpContextAccessor,
            AutoMapperConfig.Initialize(),
            _localizer);
    }
}
