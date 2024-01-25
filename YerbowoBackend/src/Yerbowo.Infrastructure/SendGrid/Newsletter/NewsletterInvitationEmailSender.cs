namespace Yerbowo.Infrastructure.SendGrid.Newsletter;

[ExcludeFromCodeCoverage]
public class NewsletterInvitationEmailSender : EmailTemplateSenderBase, INewsletterInvitationEmailSender
{
    public NewsletterInvitationEmailSender(IOptions<SendGridSettings> settings)
        : base(settings, settings.Value.NewsletterInvitationEmailTemplateId)
    {
    }
}