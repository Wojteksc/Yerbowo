namespace Yerbowo.Application.Abstractions.Emails;

public interface IEmailService<TTemplate>
{
    public Task SendAsync(string to, object dynamicTemplateData);
}