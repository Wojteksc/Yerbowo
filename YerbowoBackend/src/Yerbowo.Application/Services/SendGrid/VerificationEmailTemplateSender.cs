using Yerbowo.Application.Settings;

namespace Yerbowo.Application.Services.SendGrid;

public class VerificationEmailTemplateSender : EmailTemplateSenderBase, IVerificationEmailTemplateSender
{
    public VerificationEmailTemplateSender(IOptions<SendGridSettings> settings)
        : base(settings, settings.Value.VerificationEmailTemplateId) { }
}