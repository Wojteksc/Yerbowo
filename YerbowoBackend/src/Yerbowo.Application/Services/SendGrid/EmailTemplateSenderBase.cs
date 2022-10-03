using Yerbowo.Application.Settings;

namespace Yerbowo.Application.Services.SendGrid;

public abstract class EmailTemplateSenderBase : ISendGridEmailSender
{
    protected string _templateId;
    private readonly SendGridSettings _settings;

    public EmailTemplateSenderBase(IOptions<SendGridSettings> settings, string templateId)
    {
        _settings = settings.Value;
        _templateId = templateId;
    }

    public virtual async Task<Response> SendEmailAsync(EmailAddress to, object dynamicTemplateData)
    {
        var client = new SendGridClient(_settings.ApiKey);
        var from = new EmailAddress(_settings.SenderEMail, _settings.SenderName);
        var msg = MailHelper.CreateSingleTemplateEmail(from, to, _templateId, dynamicTemplateData);

        return await client.SendEmailAsync(msg);
    }
}