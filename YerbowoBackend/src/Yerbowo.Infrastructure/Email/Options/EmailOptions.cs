namespace Yerbowo.Infrastructure.Email.Options;

[ExcludeFromCodeCoverage]
public class EmailOptions
{
    public string Provider { get; set; }
    public string SenderEmail { get; set; }
    public string SenderName { get; set; }
    public SendGridOptions SendGrid { get; set; }
    public MailgunOptions Mailgun { get; set; }

    public string GetTemplateId(string templateName)
    {
        return Provider switch
        {
            "SendGrid" => SendGrid.TemplateIds[templateName],
            "Mailgun" => Mailgun.TemplateIds[templateName],
            _ => throw new InvalidOperationException($"Unknown provider: {Provider}")
        };
    }
}