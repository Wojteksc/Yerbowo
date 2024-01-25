namespace Yerbowo.Unit.Tests.Application.Newsletters.Command;

public class InviteNewsletterHandlerTest
{
    private readonly Mock<INewsletterRepository> _newsletterRepositoryMock;
    private readonly Mock<IWebEncoder> _webEncoderMock;

    private readonly InviteNewsletterHandler _handler;

    public InviteNewsletterHandlerTest()
    {
        _newsletterRepositoryMock = new Mock<INewsletterRepository>();
        _webEncoderMock = new Mock<IWebEncoder>();

        _handler = new InviteNewsletterHandler(
            _newsletterRepositoryMock.Object,
            StringLocalizerFactory.Create(),
            _webEncoderMock.Object);
    }

    [Theory]
    [InlineData("en-US", "Thank you for signing up to the newsletter. We have sent a message to the e-mail.")]
    [InlineData("pl-PL", "Dziękujemy za zapis do newslettera. Wysłaliśmy wiadomość na podany adres e-mail.")]
    public async Task Should_InviteNewsletter(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        string token = "token";

        var expectedNewsletter = Newsletter.Create("test@test.ts", token);

        var request = new InviteNewsletterCommand() { Email = "test@test.ts" };

        _newsletterRepositoryMock
            .Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync((Newsletter)null);

        _webEncoderMock
            .Setup(x => x.Base64UrlEncodeGuid())
            .Returns(token);

        var newsletters = new List<Newsletter>();

        _newsletterRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Newsletter>()))
            .Callback<Newsletter>(n => newsletters.Add(n));

        var result = await _handler.Handle(request, default);

        result.Should().Be(expectedMessage);
        _newsletterRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Newsletter>()), Times.Once());
        newsletters.Should().AllBeEquivalentTo(expectedNewsletter);
    }

    [Theory]
    [InlineData("en-US", "This email address is already subscribed")]
    [InlineData("pl-PL", "Ten adres e-mail jest już subskrybowany")]
    public async Task Should_ThrowException_WhenEmailIsAlreadySubscribed(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        var request = new InviteNewsletterCommand() { Email = "test@test.ts" };

        var newsletter = Newsletter.Create(request.Email, "token");
        newsletter.Subscribe();

        _newsletterRepositoryMock
            .Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync(newsletter);

        Func<Task> act = () => _handler.Handle(request, It.IsAny<CancellationToken>());

        var exception = await Assert.ThrowsAsync<Exception>(act);
        exception.Message.Should().Be(expectedMessage);
        //TO DO: Verify NOT SaveAllChangesAsync
    }
}