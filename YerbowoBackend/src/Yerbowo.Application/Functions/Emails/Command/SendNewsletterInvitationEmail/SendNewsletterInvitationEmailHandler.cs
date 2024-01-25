namespace Yerbowo.Application.Functions.Emails.Command.SendNewsletterInvitationEmail;

public class SendNewsletterInvitationEmailHandler : IRequestHandler<SendNewsletterInvitationEmailCommand>
{
    private readonly INewsletterInvitationEmailSender _emailSender;
    private readonly AppSettings _appSettings;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public SendNewsletterInvitationEmailHandler(
        INewsletterInvitationEmailSender emailSender,
        IOptions<AppSettings> options,
        IStringLocalizer<SharedResource> localizer)
    {
        _emailSender = emailSender;
        _appSettings = options.Value;
        _localizer = localizer;
    }

    public async Task Handle(SendNewsletterInvitationEmailCommand request, CancellationToken cancellationToken)
    {
        object dynamicTemplateData = new
        {
            ConfirmationLink = $"{_appSettings.BaseUrl}/subscribe?email={request.Email}&token={request.VerificationToken}"
        };

        var responseEmail = await _emailSender.SendEmailAsync(new EmailAddress(request.Email), dynamicTemplateData);

        if (!responseEmail.IsSuccessStatusCode)
        {
            throw new Exception(_localizer["ExceptionFailedAttemptToSendEmail"]);
        }
    }
}