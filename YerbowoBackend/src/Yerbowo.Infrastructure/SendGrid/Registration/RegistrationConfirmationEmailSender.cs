namespace Yerbowo.Infrastructure.SendGrid.Registration;

[ExcludeFromCodeCoverage]
public class RegistrationConfirmationEmailSender : EmailTemplateSenderBase, IRegistrationConfirmationEmailSender
{
    public RegistrationConfirmationEmailSender(IOptions<SendGridSettings> settings)
        : base(settings, settings.Value.RegistrationConfirmationEmailTemplateId) { }
}