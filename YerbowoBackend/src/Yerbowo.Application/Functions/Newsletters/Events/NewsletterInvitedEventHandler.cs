namespace Yerbowo.Application.Functions.Newsletters.Events;

[ExcludeFromCodeCoverage]
public class NewsletterInvitedEventHandler : INotificationHandler<NewsletterInvitedDomainEvent>
{
    private readonly IMediator _mediator;

    public NewsletterInvitedEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(NewsletterInvitedDomainEvent @event, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendNewsletterInvitationEmailCommand(@event.Email, @event.VerificationToken));
    }
}