namespace Yerbowo.Unit.Tests.Application.Products.Command;

public class CreateProductHandlerTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly CreateProductHandler _handler;
    private readonly CreateProductCommand _request;

    public CreateProductHandlerTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();

        _handler = new CreateProductHandler(
            _productRepositoryMock.Object,
            AutoMapperConfig.Initialize());

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

        _productRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Product>()))
            .Callback<Product>(p => products.Add(p));

        var result = await _handler.Handle(_request, CancellationToken.None);

        _productRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once());
        result.Should().BeEquivalentTo(expectedResult);
        products.Should().AllBeEquivalentTo(expectedInsertedProduct);
    }

    [Fact]
    public async Task Should_ThrowException_When_ProductNameAlreadyExists()
    {
        _productRepositoryMock.Setup(x => x.ExistsAsync(_request.Name.ToSlug()))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be($"Produkt o nazwie {_request.Name} już istnieje. Zmień nazwę.");
        _productRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Never);
    }
}