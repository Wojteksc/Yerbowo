namespace Yerbowo.Infrastructure.Email;

[ExcludeFromCodeCoverage]
public class EmailService<TTemplate> : IEmailService<TTemplate>
{
    private readonly IEmailClient emailClient;
    private readonly string templateId;

    public EmailService(IEmailClient emailClient, IOptions<EmailOptions> options)
    {
        this.emailClient = emailClient;

        string templateName = typeof(TTemplate).Name;
        templateId = options.Value.GetTemplateId(templateName);
    }

    public virtual async Task SendAsync(string toEmail, object dynamicTemplateData)
        => await emailClient.SendAsync(templateId, toEmail, dynamicTemplateData);
}