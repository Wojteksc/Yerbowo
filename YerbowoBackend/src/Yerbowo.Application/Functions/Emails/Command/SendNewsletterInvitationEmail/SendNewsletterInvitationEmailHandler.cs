namespace Yerbowo.Application.Functions.Emails.Command.SendNewsletterInvitationEmail;

public class SendNewsletterInvitationEmailHandler(
    IEmailService<NewsletterInvitationTemplate> emailService,
    IAppSettings settings) : ICommandHandler<SendNewsletterInvitationEmailCommand>
{
    public async Task Handle(SendNewsletterInvitationEmailCommand request, CancellationToken cancellationToken)
    {
        object dynamicTemplateData = new
        {
            ConfirmationLink = $"{settings.BaseUrl}/newsletter/subscribe?email={request.Email}&token={request.VerificationToken}"
        };

        await emailService.SendAsync(request.Email, dynamicTemplateData);
    }
}