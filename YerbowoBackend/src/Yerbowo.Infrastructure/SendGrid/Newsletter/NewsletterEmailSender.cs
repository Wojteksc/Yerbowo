namespace Yerbowo.Infrastructure.SendGrid.Newsletter;

[ExcludeFromCodeCoverage]
public class NewsletterEmailSender : EmailTemplateSenderBase, INewsletterEmailSender
{
    public NewsletterEmailSender(IOptions<SendGridSettings> settings)
        : base(settings, settings.Value.NewsletterEmailTemplateId)
    {
    }
}