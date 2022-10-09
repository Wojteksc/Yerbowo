using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Yerbowo.Application.Functions.Emails.Command.SendVerificationEmails;
using Yerbowo.Application.Services.SendGrid;
using Yerbowo.Domain.Users;

namespace Yerbowo.Unit.Tests.Application.Emails.Command;

public class SendVerificationEmailHandlerTest
{
    private readonly Mock<IVerificationEmailTemplateSender> _verificationEmailTemplateSender;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;

    private readonly User user;
    private readonly SendVerificationEmailCommand sendVerificationEmailCommand;

    public SendVerificationEmailHandlerTest()
    {
        _verificationEmailTemplateSender = new Mock<IVerificationEmailTemplateSender>();
        _httpContextAccessor = new Mock<IHttpContextAccessor>();

        user = new User("firstName", "lastName", "email@email.com", "companyName",
"role", "photoUrl", "provider", "password");
        user.SetVerificationToken(WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray()));
        sendVerificationEmailCommand = new SendVerificationEmailCommand(user);
    }

    [Fact]
    public async Task Should_SendEmail_When_DataAreCorrect()
    {
        var emailAddress = new EmailAddress(user.Email, user.FirstName);

        _verificationEmailTemplateSender.Setup(
            x => x.SendEmailAsync(It.IsAny<EmailAddress>(), It.IsAny<object>()))
            .ReturnsAsync(new SendGrid.Response(HttpStatusCode.OK, It.IsAny<HttpContent>(), It.IsAny<HttpResponseHeaders>()));

        _httpContextAccessor.Setup(x => x.HttpContext.Request.Headers["Referer"])
            .Returns("http://localhost:4200");

        var handler = new SendVerificationEmailHandler(
            _verificationEmailTemplateSender.Object,
            _httpContextAccessor.Object);

        Func<Task> act = () => handler.Handle(sendVerificationEmailCommand, It.IsAny<CancellationToken>());
        await act.Should().NotThrowAsync();
        _verificationEmailTemplateSender.Verify(
            x =>
            x.SendEmailAsync(It.IsAny<EmailAddress>(), It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task Should_ThrowException_When_EmailCouldntBeSent()
    {

        _verificationEmailTemplateSender.Setup(
            x => x.SendEmailAsync(It.IsAny<EmailAddress>(), It.IsAny<object>()))
            .ReturnsAsync(new SendGrid.Response(HttpStatusCode.Conflict, It.IsAny<HttpContent>(), It.IsAny<HttpResponseHeaders>()));

        _httpContextAccessor.Setup(x => x.HttpContext.Request.Headers["Referer"])
            .Returns("http://localhost:4200/");

        var handler = new SendVerificationEmailHandler(
            _verificationEmailTemplateSender.Object,
            _httpContextAccessor.Object);

        Func<Task> act = () => handler.Handle(sendVerificationEmailCommand, It.IsAny<CancellationToken>());

        var exception = await Assert.ThrowsAsync<Exception>(act);
        Assert.Equal("Nieudana próba wysłania e-maila.", exception.Message);
    }
}