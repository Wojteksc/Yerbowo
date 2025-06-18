namespace Yerbowo.Infrastructure.SendGrid.Newsletter;

[ExcludeFromCodeCoverage]
public class NewsletterEmailSender : EmailTemplateSenderBase, INewsletterEmailSender
{
    public NewsletterEmailSender(IOptions<SendGridOptions> options)
        : base(options, options.Value.NewsletterEmailTemplateId)
    {
    }
}