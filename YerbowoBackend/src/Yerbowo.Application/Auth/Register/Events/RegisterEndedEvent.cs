using Yerbowo.Application.Emails;
using Yerbowo.Domain.Users;

namespace Yerbowo.Application.Auth.Register.Events;

public class RegisterEndedEvent : INotification
{
    public User User { get; }

    public RegisterEndedEvent(User user)
    {
        User = user;
    }
}