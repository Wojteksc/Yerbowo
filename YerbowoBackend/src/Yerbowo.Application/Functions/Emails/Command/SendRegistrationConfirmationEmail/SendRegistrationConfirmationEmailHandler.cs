namespace Yerbowo.Application.Functions.Emails.Command.SendRegistrationConfirmationEmail;

public class SendRegistrationConfirmationEmailHandler(
    IEmailService<RegistrationConfirmationTemplate> emailService,
    IAppSettings appSettings) : ICommandHandler<SendRegistrationConfirmationEmailCommand>
{
    public async Task Handle(SendRegistrationConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        object dynamicTemplateData = new
        {
            RecipientName = request.FirstName,
            ConfirmationLink = $"{appSettings.BaseUrl}/auth/potwierdz-email?email={request.Email}&token={request.VerificationToken}"
        };

        await emailService.SendAsync(request.Email, dynamicTemplateData);
    }
}