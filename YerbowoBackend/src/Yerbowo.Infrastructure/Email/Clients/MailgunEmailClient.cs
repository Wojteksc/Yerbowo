namespace Yerbowo.Infrastructure.Email.Clients;

[ExcludeFromCodeCoverage]
public class MailgunEmailClient : IEmailClient
{
    private readonly IStringLocalizer<SharedResource> localizer;
    private readonly EmailOptions emailOptions;
    private readonly RestClient restClient;

    public MailgunEmailClient(IOptions<EmailOptions> options, IStringLocalizer<SharedResource> localizer)
    {
        this.localizer = localizer;
        emailOptions = options.Value;
        var optionsRest = new RestClientOptions(emailOptions.Mailgun.ApiBaseUrl)
        {
            Authenticator = new HttpBasicAuthenticator("api", emailOptions.Mailgun.ApiKey)
        };
        restClient = new RestClient(optionsRest);
    }

    public async Task SendAsync(string templateId, string toEmail, object dynamicTemplateData)
    {
        var request = new RestRequest(emailOptions.Mailgun.DomainPath, Method.Post);
        request.AlwaysMultipartFormData = true;
        request.AddParameter("from", $"{emailOptions.SenderName} <{emailOptions.SenderEmail}>");
        request.AddParameter("to", $"<{toEmail}>");
        request.AddParameter("template", templateId);
        request.AddParameter("h:X-Mailgun-Variables", JsonSerializer.Serialize(dynamicTemplateData));

        var response = await restClient.ExecuteAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new EmailSendingFailedException(localizer);
        }
    }
}