namespace Yerbowo.Unit.Tests.Application.Emails.Command;

public class SendRegistrationConfirmationEmailHandlerTest
{
    private readonly Mock<IEmailService<RegistrationConfirmationTemplate>> emailService;

    private readonly SendRegistrationConfirmationEmailCommand request;

    public SendRegistrationConfirmationEmailHandlerTest()
    {
        emailService = new();

        request = new SendRegistrationConfirmationEmailCommand("firstName", "email@email.com", "token");
    }

    [Fact]
    public async Task Should_SendEmaiCorrectly()
    {
        var emailAddress = new EmailAddress(request.Email, "firstName");

        emailService.Setup(
            x => x.SendAsync(It.IsAny<string>(), It.IsAny<object>()));

        IAppSettings appSettings = new AppOptions() { BaseUrl = "http://localhost:5000" };

        var handler = new SendRegistrationConfirmationEmailHandler(
            emailService.Object,
            appSettings);

        Func<Task> act = () => handler.Handle(request, It.IsAny<CancellationToken>());
        await act.Should().NotThrowAsync();
        emailService.Verify(
            x =>
            x.SendAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
    }
}