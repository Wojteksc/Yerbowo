namespace Yerbowo.Application.Functions.Emails.Command.SendNewsletterInvitationEmail;

public class SendNewsletterInvitationEmailCommand : IRequest
{
    public string Email { get; }
    public string VerificationToken { get; }

    public SendNewsletterInvitationEmailCommand(string email, string verificationToken)
    {
        Email = email;
        VerificationToken = verificationToken;
    }
}