namespace Yerbowo.Application.Emails;

public class EmailTemplateData
{
    public string RecipientName { get; }
    public string ConfirmationLink { get; }

    public EmailTemplateData(string recipientName, string confirmationLink)
    {
        RecipientName = recipientName;
        ConfirmationLink = confirmationLink;
    }
}