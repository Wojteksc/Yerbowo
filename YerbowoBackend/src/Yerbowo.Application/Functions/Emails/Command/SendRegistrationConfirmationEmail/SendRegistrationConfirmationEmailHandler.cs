namespace Yerbowo.Application.Functions.Emails.Command.SendRegistrationConfirmationEmail;

public class SendRegistrationConfirmationEmailHandler : IRequestHandler<SendRegistrationConfirmationEmailCommand>
{
    private readonly IRegistrationConfirmationEmailSender _emailSender;
    private readonly AppSettings _appSettings;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public SendRegistrationConfirmationEmailHandler(
        IRegistrationConfirmationEmailSender emailSender,
        IOptions<AppSettings> settings,
        IStringLocalizer<SharedResource> localizer)
    {
        _emailSender = emailSender;
        _appSettings = settings.Value;
        _localizer = localizer;
    }

    public async Task Handle(SendRegistrationConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        object dynamicTemplateData = new
        {
            RecipientName = request.FirstName,
            ConfirmationLink = $"{_appSettings.BaseUrl}/confirmEmail?email={request.Email}&token={request.VerificationToken}"
        };

        var responseEmail = await _emailSender.SendEmailAsync(
            new EmailAddress(request.Email, request.FirstName),
            dynamicTemplateData
        );

        if (!responseEmail.IsSuccessStatusCode)
        {
            throw new Exception(_localizer["ExceptionFailedAttemptToSendEmail"]);
        }
    }
}