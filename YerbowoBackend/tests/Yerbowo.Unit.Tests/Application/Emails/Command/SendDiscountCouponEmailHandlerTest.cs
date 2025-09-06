namespace Yerbowo.Unit.Tests.Application.Emails.Command;

public class SendDiscountCouponEmailHandlerTest
{
    private readonly Mock<IEmailService<NewsletterCouponTemplate>> emailService;

    private readonly SendDiscountCouponEmailCommand request;


    public SendDiscountCouponEmailHandlerTest()
    {
        emailService = new();

        request = new SendDiscountCouponEmailCommand("email@email.com", "token");
    }

    [Fact]
    public async Task Should_SendEmaiCorrectly()
    {
        var emailAddress = new EmailAddress("email@email.com");

        emailService
            .Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<object>()));

        IAppSettings appSettings = new AppOptions() { BaseUrl = "http://localhost:5000" };

        var handler = new SendDiscountCouponEmailHandler(
            emailService.Object,
            appSettings);

        Func<Task> act = () => handler.Handle(request, It.IsAny<CancellationToken>());
        await act.Should().NotThrowAsync();
        emailService.Verify(
            x =>
            x.SendAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
    }
}