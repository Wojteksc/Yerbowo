using Yerbowo.Application.Services.SendGrid;

namespace Yerbowo.Application.Emails.SendVerificationEmails;

public class SendVerificationEmailHandler : IRequestHandler<SendVerificationEmailCommand>
{
    private readonly IVerificationEmailTemplateSender _verificationEmailTemplateSender;
    private readonly HttpRequest _httpRequest;

    public SendVerificationEmailHandler(
        IVerificationEmailTemplateSender verificationEmailTemplateSender,
        IHttpContextAccessor httpContextAccessor)
    {
        _verificationEmailTemplateSender = verificationEmailTemplateSender;
        _httpRequest = httpContextAccessor.HttpContext.Request;
    }

    public async Task<Unit> Handle(SendVerificationEmailCommand command, CancellationToken cancellationToken)
    {
        var user = command.User;
        var domainUrl = $"{_httpRequest.Scheme}://{_httpRequest.Host.Value}";

        object dynamicTemplateData = new
        {
            RecipientName = command.User.FirstName,
            ConfirmationLink = $"{domainUrl}/confirmEmail?email={user.Email}&token={user.VerificationToken}"
        };

        var responseEmail = await _verificationEmailTemplateSender.SendEmailAsync(
            new EmailAddress(user.Email, user.FirstName),
            dynamicTemplateData
        );

        if (!responseEmail.IsSuccessStatusCode)
        {
            throw new Exception("Nieudana próba wysłania e-maila.");
        }

        return Unit.Value;
    }
}