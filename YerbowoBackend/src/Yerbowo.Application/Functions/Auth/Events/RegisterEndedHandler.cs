namespace Yerbowo.Application.Functions.Auth.Events;

public class RegisterEndedHandler : INotificationHandler<RegisterEndedEvent>
{
    private readonly IMediator _mediator;

    public RegisterEndedHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(RegisterEndedEvent @event, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendVerificationEmailCommand(@event.User));
    }
}