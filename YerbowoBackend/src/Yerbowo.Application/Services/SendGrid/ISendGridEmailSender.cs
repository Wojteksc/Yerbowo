namespace Yerbowo.Application.Services.SendGrid;

public interface ISendGridEmailSender
{
    public Task<Response> SendEmailAsync(EmailAddress to, object dynamicTemplateData);
}