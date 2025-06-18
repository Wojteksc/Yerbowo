namespace Yerbowo.Application.Functions.Emails.Command.SendNewsletterInvitationEmail;

public class SendNewsletterInvitationEmailHandler(
    INewsletterInvitationEmailSender emailSender,
    IAppSettings settings,
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<SendNewsletterInvitationEmailCommand>
{
    public async Task Handle(SendNewsletterInvitationEmailCommand request, CancellationToken cancellationToken)
    {
        object dynamicTemplateData = new
        {
            ConfirmationLink = $"{settings.BaseUrl}/subscribe?email={request.Email}&token={request.VerificationToken}"
        };

        var responseEmail = await emailSender.SendEmailAsync(new EmailAddress(request.Email), dynamicTemplateData);

        if (!responseEmail.IsSuccessStatusCode)
        {
            throw new EmailSendingFailedException(localizer);
        }
    }
}