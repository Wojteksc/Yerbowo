namespace Yerbowo.Unit.Tests.Application.Newsletters.Command;

public class SubscribeNewsletterHandlerTest
{
	private readonly Mock<INewsletterRepository> _newsletterRepositoryMock;
	private readonly Mock<IMediator> _mediatorNock;

    private readonly Newsletter _newsletter;
    private readonly SubscribeNewsletterHandler _handler;

    public SubscribeNewsletterHandlerTest()
	{
        _newsletterRepositoryMock = new Mock<INewsletterRepository>();
        _mediatorNock = new Mock<IMediator>();

        _handler = new SubscribeNewsletterHandler(
			_mediatorNock.Object,
			_newsletterRepositoryMock.Object,
            StringLocalizerFactory.Create());

		_newsletter = Newsletter.Create("test@test.com", "token");
    }

    [Fact]
    public async Task Should_SubscribeNewsletter()
	{
		var newsletters = new List<Newsletter>();

		var request = new SubscribeNewsletterCommand()
		{
			Email = "test@test.com",
			Token = "token"
		};

        var expectedNewsletter = Newsletter.Create("test@test.com", "token");
		expectedNewsletter.Subscribe();

		_newsletterRepositoryMock
			.Setup(x => x.GetAsync(request.Email))
			.ReturnsAsync(_newsletter);

		_newsletterRepositoryMock
			.Setup(x => x.UpdateAsync(It.IsAny<Newsletter>()))
			.Callback<Newsletter>(n => newsletters.Add(n));

		await _handler.Handle(request, default);

		_newsletterRepositoryMock.Verify(x => x.GetAsync(request.Email), Times.Once());
		_newsletterRepositoryMock.Verify(x => x.UpdateAsync(newsletters.First()), Times.Once());
        newsletters.Should().AllBeEquivalentTo(expectedNewsletter, 
			options => options
			.Excluding(x => x.VerifiedAt));

		newsletters.First().Should().NotBeNull();
    }

    [Theory]
    [InlineData("en-US", "Bad request")]
    [InlineData("pl-PL", "Nieprawidłowe żądanie")]
    public async Task Should_ThrowException_WhenNewsletterDoesNotExist(string culture, string expectedMessage)
	{
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        var request = new SubscribeNewsletterCommand()
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

        var request = new SubscribeNewsletterCommand()
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

    [Theory]
    [InlineData("en-US", "The e-mail address was already confirmed")]
    [InlineData("pl-PL", "Adres e-mail został już potwierdzony")]
    public async Task Should_ThrowException_WhenEmailWasConfirmed(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        var request = new SubscribeNewsletterCommand()
        {
            Email = "test@test.com",
            Token = "token"
        };

        _newsletter.Subscribe();

        _newsletterRepositoryMock
            .Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync(_newsletter);

        Func<Task> act = () => _handler.Handle(request, It.IsAny<CancellationToken>());

        var exception = await Assert.ThrowsAsync<Exception>(act);
        exception.Message.Should().Be(expectedMessage);

        _newsletterRepositoryMock.Verify(x => x.GetAsync(request.Email), Times.Once());
        _newsletterRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Newsletter>()), Times.Never());
    }
}