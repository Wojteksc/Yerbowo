namespace Yerbowo.Application.Functions.Emails.Command.SendVerificationEmails;

public class SendVerificationEmailHandler : IRequestHandler<SendVerificationEmailCommand>
{
    private readonly IVerificationEmailTemplateSender _verificationEmailTemplateSender;
    private readonly HttpRequest _httpRequest;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public SendVerificationEmailHandler(
        IVerificationEmailTemplateSender verificationEmailTemplateSender,
        IHttpContextAccessor httpContextAccessor,
        IStringLocalizer<SharedResource> localizer)
    {
        _verificationEmailTemplateSender = verificationEmailTemplateSender;
        _httpRequest = httpContextAccessor.HttpContext.Request;
        _localizer = localizer;
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
            throw new Exception(_localizer["ExceptionFailedAttemptToSendEmail"]);
        }

        return Unit.Value;
    }
}