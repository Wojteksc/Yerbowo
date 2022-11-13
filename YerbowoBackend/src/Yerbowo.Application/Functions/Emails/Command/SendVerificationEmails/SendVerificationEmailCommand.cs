namespace Yerbowo.Application.Functions.Emails.Command.SendVerificationEmails;

public class SendVerificationEmailCommand : IRequest
{
    public User User { get; }

    public SendVerificationEmailCommand(User user)
    {
        User = user;
    }
}