namespace Yerbowo.Infrastructure.SendGrid;

[ExcludeFromCodeCoverage]
public abstract class EmailTemplateSenderBase : ISendGridEmailSender
{
    protected string templateId;
    private readonly SendGridOptions options;

    public EmailTemplateSenderBase(IOptions<SendGridOptions> settings, string templateId)
    {
        options = settings.Value;
        this.templateId = templateId;
    }

    public virtual async Task<Response> SendEmailAsync(EmailAddress to, object dynamicTemplateData)
    {
        var client = new SendGridClient(options.ApiKey);
        var from = new EmailAddress(options.SenderEMail, options.SenderName);
        var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, dynamicTemplateData);

        return await client.SendEmailAsync(msg);
    }
}