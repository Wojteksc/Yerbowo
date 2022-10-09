using Yerbowo.Domain.Users;

namespace Yerbowo.Application.Functions.Auth.Events;

public class RegisterEndedEvent : INotification
{
    public User User { get; }

    public RegisterEndedEvent(User user)
    {
        User = user;
    }
}