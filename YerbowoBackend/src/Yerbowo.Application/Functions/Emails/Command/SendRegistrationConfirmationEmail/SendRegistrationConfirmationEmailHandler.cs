namespace Yerbowo.Application.Functions.Emails.Command.SendRegistrationConfirmationEmail;

public class SendRegistrationConfirmationEmailHandler(
    IRegistrationConfirmationEmailSender emailSender,
    IAppSettings appSettings,
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<SendRegistrationConfirmationEmailCommand>
{
    public async Task Handle(SendRegistrationConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        object dynamicTemplateData = new
        {
            RecipientName = request.FirstName,
            ConfirmationLink = $"{appSettings.BaseUrl}/confirmEmail?email={request.Email}&token={request.VerificationToken}"
        };

        var responseEmail = await emailSender.SendEmailAsync(
            new EmailAddress(request.Email, request.FirstName),
            dynamicTemplateData
        );

        if (!responseEmail.IsSuccessStatusCode)
        {
            throw new EmailSendingFailedException(localizer);
        }
    }
}