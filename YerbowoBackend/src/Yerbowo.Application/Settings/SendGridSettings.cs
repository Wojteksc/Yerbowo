namespace Yerbowo.Application.Settings;

public record SendGridSettings
{
    public required string ApiKey { get; init; }
    public required string SenderEMail { get; init; }
    public required string SenderName { get; init; }
    public required string RegistrationConfirmationEmailTemplateId { get; init; }
    public required string NewsletterInvitationEmailTemplateId { get; init; }
    public required string NewsletterEmailTemplateId { get; init; }
}