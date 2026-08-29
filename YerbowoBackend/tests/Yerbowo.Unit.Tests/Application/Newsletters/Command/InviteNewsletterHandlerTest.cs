namespace Yerbowo.Unit.Tests.Application.Newsletters.Command;

public class InviteNewsletterHandlerTest
{
    private readonly Mock<INewsletterRepository> _newsletterRepository = new();
    private readonly Mock<IWebEncoder> _webEncoder = new();
    private readonly Mock<IIdGenerator> _idGenerator = new();

    private readonly InviteNewsletterHandler _handler;

    private IStringLocalizer<SharedResource> _localizer;

    Guid NewsletterId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public InviteNewsletterHandlerTest()
    {
        _localizer = StringLocalizerFactory.Create();

        _handler = new InviteNewsletterHandler(
            _newsletterRepository.Object,
            _localizer,
            _webEncoder.Object,
            _idGenerator.Object);

        _idGenerator
            .Setup(x => x.Generate())
            .Returns(NewsletterId);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_InviteNewsletter(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.NewsletterSentEmailResponse];

        string token = "token";

        var expectedNewsletter = Newsletter.Create(NewsletterId, "test@test.ts", token);

        var request = new InviteNewsletterCommand() { Email = "test@test.ts" };

        _newsletterRepository
            .Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync((Newsletter)null);

        _webEncoder
            .Setup(x => x.Base64UrlEncodeGuid())
            .Returns(token);

        Newsletter newsletter = null;
        _newsletterRepository
            .Setup(x => x.AddAsync(It.IsAny<Newsletter>()))
            .Callback<Newsletter>(n => newsletter = n);

        string response = await _handler.Handle(request, default);

        response.Should().Be(_localizer[Localizations.NewsletterSentEmailResponse].Value);
        _newsletterRepository.Verify(x => x.AddAsync(It.IsAny<Newsletter>()), Times.Once());
        _newsletterRepository.Verify(x => x.UpdateAsync(It.IsAny<Newsletter>()), Times.Never());
        newsletter.Should().BeEquivalentTo(expectedNewsletter);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_WhenEmailIsAlreadySubscribed(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.EmailIsAlreadySubscribed];

        var request = new InviteNewsletterCommand() { Email = "test@test.ts" };

        var newsletter = Newsletter.Create(NewsletterId, request.Email, "token");
        newsletter.Subscribe();

        _newsletterRepository
            .Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync(newsletter);

        Func<Task> act = () => _handler.Handle(request, It.IsAny<CancellationToken>());

        var exception = await Assert.ThrowsAsync<EmailIsAlreadySubscribedToNewsletterException>(act);
        exception.Message.Should().Be(expectedMessage);
        _newsletterRepository.Verify(x => x.UpdateAsync(It.IsAny<Newsletter>()), Times.Never);
    }
}