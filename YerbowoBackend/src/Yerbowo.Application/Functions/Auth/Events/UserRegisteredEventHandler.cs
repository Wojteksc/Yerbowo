namespace Yerbowo.Application.Functions.Auth.Events;

[ExcludeFromCodeCoverage]
public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredDomainEvent>
{
    private readonly IMediator _mediator;

    public UserRegisteredEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(UserRegisteredDomainEvent @event, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new SendRegistrationConfirmationEmailCommand(
                @event.FirstName, @event.Email, @event.VeriicationToken));
    }
}