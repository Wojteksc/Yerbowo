namespace Yerbowo.Unit.Tests.Application.Emails.Command;

public class SendRegistrationConfirmationEmailHandlerTest
{
    private readonly Mock<IRegistrationConfirmationEmailSender> _emailSender;

    private readonly SendRegistrationConfirmationEmailCommand request;

    private IStringLocalizer<SharedResource> _localizer;

    public SendRegistrationConfirmationEmailHandlerTest()
    {
        _emailSender = new();

        _localizer = StringLocalizerFactory.Create();

        request = new SendRegistrationConfirmationEmailCommand("firstName", "email@email.com", "token");
    }

    [Fact]
    public async Task Should_SendEmaiCorrectly()
    {
        var emailAddress = new EmailAddress(request.Email, "firstName");

        _emailSender.Setup(
            x => x.SendEmailAsync(It.IsAny<EmailAddress>(), It.IsAny<object>()))
            .ReturnsAsync(new SendGrid.Response(HttpStatusCode.OK, It.IsAny<HttpContent>(), It.IsAny<HttpResponseHeaders>()));

        IAppSettings appSettings = new AppOptions() { BaseUrl = "http://localhost:5000" };

        var handler = new SendRegistrationConfirmationEmailHandler(
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
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_EmailCouldntBeSent(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.EmailSendingFailedException];

        _emailSender.Setup(
            x => x.SendEmailAsync(It.IsAny<EmailAddress>(), It.IsAny<object>()))
            .ReturnsAsync(new SendGrid.Response(HttpStatusCode.Conflict, It.IsAny<HttpContent>(), It.IsAny<HttpResponseHeaders>()));

        IAppSettings appSettings = new AppOptions() { BaseUrl = "http://localhost:5000" };

        var handler = new SendRegistrationConfirmationEmailHandler(
            _emailSender.Object,
            appSettings,
            StringLocalizerFactory.Create());

        Func<Task> act = () => handler.Handle(request, It.IsAny<CancellationToken>());

        var exception = await Assert.ThrowsAsync<EmailSendingFailedException>(act);
        exception.Message.Should().Be(expectedMessage);
    }
}