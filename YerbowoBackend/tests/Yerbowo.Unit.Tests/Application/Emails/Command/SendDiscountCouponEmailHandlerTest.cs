namespace Yerbowo.Unit.Tests.Application.Emails.Command;

public class SendDiscountCouponEmailHandlerTest
{
    private readonly Mock<INewsletterEmailSender> _emailSender;

    private readonly SendDiscountCouponEmailCommand request;

    private IStringLocalizer<SharedResource> _localizer;

    public SendDiscountCouponEmailHandlerTest()
    {
        _emailSender = new();

        _localizer = StringLocalizerFactory.Create();

        request = new SendDiscountCouponEmailCommand("email@email.com", "token");
    }

    [Fact]
    public async Task Should_SendEmaiCorrectly()
    {
        var emailAddress = new EmailAddress("email@email.com");

        _emailSender
            .Setup(x => x.SendEmailAsync(It.IsAny<EmailAddress>(), It.IsAny<object>()))
            .ReturnsAsync(new SendGrid.Response(HttpStatusCode.OK, It.IsAny<HttpContent>(), It.IsAny<HttpResponseHeaders>()));

        IAppSettings appSettings = new AppOptions() { BaseUrl = "http://localhost:5000" };

        var handler = new SendDiscountCouponEmailHandler(
            _emailSender.Object,
            appSettings,
            _localizer);

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

        _emailSender
            .Setup(x => x.SendEmailAsync(It.IsAny<EmailAddress>(), It.IsAny<object>()))
            .ReturnsAsync(new SendGrid.Response(HttpStatusCode.Conflict, It.IsAny<HttpContent>(), It.IsAny<HttpResponseHeaders>()));

        IAppSettings appSettings = new AppOptions() { BaseUrl = "http://localhost:5000" };

        var handler = new SendDiscountCouponEmailHandler(
            _emailSender.Object,
            appSettings,
            _localizer);

        Func<Task> act = () => handler.Handle(request, It.IsAny<CancellationToken>());

        var exception = await Assert.ThrowsAsync<EmailSendingFailedException>(act);
        exception.Message.Should().Be(expectedMessage);
    }
}