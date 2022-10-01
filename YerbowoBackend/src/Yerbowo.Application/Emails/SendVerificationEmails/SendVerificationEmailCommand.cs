using Yerbowo.Domain.Users;

namespace Yerbowo.Application.Emails.SendVerificationEmails;

public class SendVerificationEmailCommand : IRequest
{
    public User User { get; }

    public SendVerificationEmailCommand(User user)
    {
        User = user;
    }
}
