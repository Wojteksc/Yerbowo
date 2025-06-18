namespace Yerbowo.Infrastructure.SendGrid.Newsletter;

[ExcludeFromCodeCoverage]
public class NewsletterInvitationEmailSender : EmailTemplateSenderBase, INewsletterInvitationEmailSender
{
    public NewsletterInvitationEmailSender(IOptions<SendGridOptions> options)
        : base(options, options.Value.NewsletterInvitationEmailTemplateId)
    {
    }
}