namespace Yerbowo.Application.Functions.Auth.Events;

[ExcludeFromCodeCoverage]
public class UserRegisteredEventHandler(IRequestDispatcher dispatcher) : INotificationHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(UserRegisteredDomainEvent @event, CancellationToken cancellationToken)
    {
        await dispatcher.ExecuteCommand(
            new SendRegistrationConfirmationEmailCommand(
                @event.FirstName, @event.Email, @event.VerificationToken));
    }
}