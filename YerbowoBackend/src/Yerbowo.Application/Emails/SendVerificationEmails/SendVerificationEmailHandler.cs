using Yerbowo.Application.Services.SendGrid;

namespace Yerbowo.Application.Emails.SendVerificationEmails;

public class SendVerificationEmailHandler : IRequestHandler<SendVerificationEmailCommand>
{
    private readonly IVerificationEmailTemplateSender _verificationEmailTemplateSender;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SendVerificationEmailHandler(
        IVerificationEmailTemplateSender verificationEmailTemplateSender,
        IHttpContextAccessor httpContextAccessor)
    {
        _verificationEmailTemplateSender = verificationEmailTemplateSender;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Unit> Handle(SendVerificationEmailCommand command, CancellationToken cancellationToken)
    {
        var user = command.User;
        var clientUrl = _httpContextAccessor.HttpContext.Request.Headers["Referer"];

        object dynamicTemplateData = new
        {
            RecipientName = command.User.FirstName,
            ConfirmationLink = $"{clientUrl}/confirmEmail?email={user.Email}&token={user.VerificationToken}"
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