namespace Yerbowo.Unit.Tests.Application.Newsletters.Command;

public class UnsubscribeNewsletterHandlerTest
{
	private readonly Mock<INewsletterRepository> _newsletterRepositoryMock;

    private readonly Newsletter _newsletter;
    private readonly UnsubscribeNewsletterHandler _handler;

    private IStringLocalizer<SharedResource> _localizer;

    public UnsubscribeNewsletterHandlerTest()
    {
        _newsletterRepositoryMock = new();

        _localizer = StringLocalizerFactory.Create();

        _handler = new UnsubscribeNewsletterHandler(
            _newsletterRepositoryMock.Object,
            StringLocalizerFactory.Create());

        _newsletter = Newsletter.Create("test@test.com", "token");
    }

    [Fact]
    public async Task Should_UnsubscribeNewsletter()
    {
        var newsletters = new List<Newsletter>();

        var request = new UnsubscribeNewsletterCommand("test@test.com", "token");

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
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_WhenNewsletterDoesNotExist(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.NewsletterNotFound];

        var request = new UnsubscribeNewsletterCommand("incorrectEmail@test.com", "token");

        _newsletterRepositoryMock
            .Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync((Newsletter)null);

        Func<Task> act = () => _handler.Handle(request, It.IsAny<CancellationToken>());

        var exception = await Assert.ThrowsAsync<NewsletterNotFoundException>(act);
        exception.Message.Should().Be(expectedMessage);

        _newsletterRepositoryMock.Verify(x => x.GetAsync(request.Email), Times.Once());
        _newsletterRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Newsletter>()), Times.Never());
    }

    [Theory]
    [InlineData ("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_WhenNewsletterHasIncorrectToken(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.InvalidToken];

        var request = new UnsubscribeNewsletterCommand("test@test.com", "incorrectToken");

        _newsletterRepositoryMock
            .Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync(_newsletter);

        Func<Task> act = () => _handler.Handle(request, It.IsAny<CancellationToken>());

        var exception = await Assert.ThrowsAsync<InvalidTokenException>(act);
        exception.Message.Should().Be(expectedMessage);

        _newsletterRepositoryMock.Verify(x => x.GetAsync(request.Email), Times.Once());
        _newsletterRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Newsletter>()), Times.Never());
    }
}