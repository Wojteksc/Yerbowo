namespace Yerbowo.Application.Settings;

public class SendGridSettings
{
    public string ApiKey { get; set; }
    public string SenderEMail { get; set; }
    public string SenderName { get; set; }
    public string VerificationEmailTemplateId { get; set; }
}