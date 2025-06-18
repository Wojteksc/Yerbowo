namespace Yerbowo.Unit.Tests.Application.Newsletters.Command;

public class InviteNewsletterHandlerTest
{
    private readonly Mock<INewsletterRepository> _newsletterRepository;
    private readonly Mock<IWebEncoder> _webEncoder;

    private readonly InviteNewsletterHandler _handler;

    private IStringLocalizer<SharedResource> _localizer;

    public InviteNewsletterHandlerTest()
    {
        _newsletterRepository = new();
        _webEncoder = new();


        _localizer = StringLocalizerFactory.Create();

        _handler = new InviteNewsletterHandler(
            _newsletterRepository.Object,
            _localizer,
            _webEncoder.Object);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_InviteNewsletter(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.NewsletterSentEmailResponse];

        string token = "token";

        var expectedNewsletter = Newsletter.Create("test@test.ts", token);

        var request = new InviteNewsletterCommand() { Email = "test@test.ts" };

        _newsletterRepository
            .Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync((Newsletter)null);

        _webEncoder
            .Setup(x => x.Base64UrlEncodeGuid())
            .Returns(token);

        var newsletters = new List<Newsletter>();

        _newsletterRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Newsletter>()))
            .Callback<Newsletter>(n => newsletters.Add(n));

        string response = await _handler.Handle(request, default);

        response.Should().Be(_localizer[Localizations.NewsletterSentEmailResponse].Value);
        _newsletterRepository.Verify(x => x.UpdateAsync(It.IsAny<Newsletter>()), Times.Once());
        newsletters.Should().AllBeEquivalentTo(expectedNewsletter);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_WhenEmailIsAlreadySubscribed(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.EmailIsAlreadySubscribed];

        var request = new InviteNewsletterCommand() { Email = "test@test.ts" };

        var newsletter = Newsletter.Create(request.Email, "token");
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