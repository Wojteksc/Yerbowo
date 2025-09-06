namespace Yerbowo.Infrastructure.Email.Clients;

public interface IEmailClient
{
    Task SendAsync(string templateId, string toEmail, object dynamicTemplateData);
}