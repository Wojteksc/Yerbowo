namespace Yerbowo.Unit.Tests.Application.Newsletters.Command;

public class UnsubscribeNewsletterHandlerTest
{
	private readonly Mock<INewsletterRepository> _newsletterRepositoryMock;

    private readonly Newsletter _newsletter;
    private readonly UnsubscribeNewsletterHandler _handler;

    public UnsubscribeNewsletterHandlerTest()
    {
        _newsletterRepositoryMock = new Mock<INewsletterRepository>();

        _handler = new UnsubscribeNewsletterHandler(
            _newsletterRepositoryMock.Object,
            StringLocalizerFactory.Create());

        _newsletter = Newsletter.Create("test@test.com", "token");
    }

    [Fact]
    public async Task Should_UnsubscribeNewsletter()
    {
        var newsletters = new List<Newsletter>();

        var request = new UnsubscribeNewsletterCommand()
        {
            Email = "test@test.com",
            Token = "token"
        };

        var expectedNewsletter = Newsletter.Create("test@test.com", "token");
        expectedNewsletter.Unsubscribe();

        _newsletterRepositoryMock
            .Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync(_newsletter);

        _newsletterRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Newsletter>()))
            .Callback<Newsletter>(n => newsletters.Add(n));

        await _handler.Handle(request, default);

        _newsletterRepositoryMock.Verify(x => x.GetAsync(request.Email), Times.Once());
        _newsletterRepositoryMock.Verify(x => x.UpdateAsync(newsletters.First()), Times.Once());
        newsletters.Should().AllBeEquivalentTo(expectedNewsletter, options => options);

        newsletters.First().Should().NotBeNull();
    }

    [Theory]
    [InlineData("en-US", "Bad request")]
    [InlineData("pl-PL", "Nieprawidłowe żądanie")]
    public async Task Should_ThrowException_WhenNewsletterDoesNotExist(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        var request = new UnsubscribeNewsletterCommand()
        {
            Email = "incorrectEmail@test.com",
            Token = "token"
        };

        _newsletterRepositoryMock
            .Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync((Newsletter)null);

        Func<Task> act = () => _handler.Handle(request, It.IsAny<CancellationToken>());

        var exception = await Assert.ThrowsAsync<ArgumentException>(act);
        exception.Message.Should().Be(expectedMessage);

        _newsletterRepositoryMock.Verify(x => x.GetAsync(request.Email), Times.Once());
        _newsletterRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Newsletter>()), Times.Never());
    }

    [Theory]
    [InlineData("en-US", "Bad request")]
    [InlineData("pl-PL", "Nieprawidłowe żądanie")]
    public async Task Should_ThrowException_WhenNewsletterHasIncorrectToken(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        var request = new UnsubscribeNewsletterCommand()
        {
            Email = "test@test.com",
            Token = "incorrectToken"
        };

        _newsletterRepositoryMock
            .Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync(_newsletter);

        Func<Task> act = () => _handler.Handle(request, It.IsAny<CancellationToken>());

        var exception = await Assert.ThrowsAsync<ArgumentException>(act);
        exception.Message.Should().Be(expectedMessage);

        _newsletterRepositoryMock.Verify(x => x.GetAsync(request.Email), Times.Once());
        _newsletterRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Newsletter>()), Times.Never());
    }
}