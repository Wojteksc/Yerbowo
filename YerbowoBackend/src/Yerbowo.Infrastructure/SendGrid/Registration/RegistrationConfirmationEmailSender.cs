namespace Yerbowo.Infrastructure.SendGrid.Registration;

[ExcludeFromCodeCoverage]
public class RegistrationConfirmationEmailSender : EmailTemplateSenderBase, IRegistrationConfirmationEmailSender
{
    public RegistrationConfirmationEmailSender(IOptions<SendGridOptions> options)
        : base(options, options.Value.RegistrationConfirmationEmailTemplateId) { }
}