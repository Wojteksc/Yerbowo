namespace Yerbowo.Unit.Tests.Application.Emails.Command;

public class SendNewsletterInvitationEmailHandlerTest
{
    private readonly Mock<INewsletterInvitationEmailSender> _emailSender;

    private readonly SendNewsletterInvitationEmailCommand request;

    public SendNewsletterInvitationEmailHandlerTest()
    {
        _emailSender = new Mock<INewsletterInvitationEmailSender>();

        request = new SendNewsletterInvitationEmailCommand("email@email.com", "token");
    }

    [Fact]
    public async Task Should_SendEmaiCorrectly()
    {
        var emailAddress = new EmailAddress("email@email.com");

        _emailSender.Setup(
            x => x.SendEmailAsync(It.IsAny<EmailAddress>(), It.IsAny<object>()))
            .ReturnsAsync(new SendGrid.Response(HttpStatusCode.OK, It.IsAny<HttpContent>(), It.IsAny<HttpResponseHeaders>()));

        IOptions<AppSettings> appSettings = Options.Create(
            new AppSettings() { BaseUrl = "http://localhost:5000" });

        var handler = new SendNewsletterInvitationEmailHandler(
            _emailSender.Object,
            appSettings,
            StringLocalizerFactory.Create());

        Func<Task> act = () => handler.Handle(request, It.IsAny<CancellationToken>());
        await act.Should().NotThrowAsync();
        _emailSender.Verify(
            x =>
            x.SendEmailAsync(It.IsAny<EmailAddress>(), It.IsAny<object>()), Times.Once);
    }

    [Theory]
    [InlineData("en-US", "Failed attempt to send email")]
    [InlineData("pl-PL", "Nieudana próba wysłania wiadomości e-mail")]
    public async Task Should_ThrowException_When_EmailCouldntBeSent(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        _emailSender.Setup(
            x => x.SendEmailAsync(It.IsAny<EmailAddress>(), It.IsAny<object>()))
            .ReturnsAsync(new SendGrid.Response(HttpStatusCode.Conflict, It.IsAny<HttpContent>(), It.IsAny<HttpResponseHeaders>()));

        IOptions<AppSettings> appSettings = Options.Create(new AppSettings() { BaseUrl = "http://localhost:5000" });

        var handler = new SendNewsletterInvitationEmailHandler(
            _emailSender.Object,
            appSettings,
            StringLocalizerFactory.Create());

        Func<Task> act = () => handler.Handle(request, It.IsAny<CancellationToken>());

        var exception = await Assert.ThrowsAsync<Exception>(act);
        exception.Message.Should().Be(expectedMessage);
    }
}