namespace Yerbowo.Application.Abstractions.Emails;

public interface ISendGridEmailSender
{
    public Task<Response> SendEmailAsync(EmailAddress to, object dynamicTemplateData);
}