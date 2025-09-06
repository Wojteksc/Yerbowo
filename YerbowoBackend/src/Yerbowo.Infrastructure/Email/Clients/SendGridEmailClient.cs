namespace Yerbowo.Infrastructure.Email.Clients;

[ExcludeFromCodeCoverage]
public class SendGridEmailClient : IEmailClient
{
    private readonly IStringLocalizer<SharedResource> localizer;
    private readonly EmailOptions options;
    private readonly SendGridClient client;
    private readonly EmailAddress from;

    public SendGridEmailClient(IOptions<EmailOptions> settings, IStringLocalizer<SharedResource> localizer)
    {
        this.localizer = localizer;
        options = settings.Value;
        client = new SendGridClient(options.SendGrid.ApiKey);
        from = new EmailAddress(options.SenderEmail, options.SenderName);
    }

    public async Task SendAsync(string templateId, string toEmail, object dynamicTemplateData)
    {
        var msg = MailHelper.CreateSingleTemplateEmail(from, new EmailAddress(toEmail), templateId, dynamicTemplateData);
        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            throw new EmailSendingFailedException(localizer);
        }
    }
}