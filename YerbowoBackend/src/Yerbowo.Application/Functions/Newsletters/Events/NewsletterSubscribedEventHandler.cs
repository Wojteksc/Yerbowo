namespace Yerbowo.Application.Functions.Newsletters.Events;

[ExcludeFromCodeCoverage]
public class NewsletterSubscribedEventHandler : INotificationHandler<NewsletterSubscribedDomainEvent>
{
    private readonly IMediator _mediator;

    public NewsletterSubscribedEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(NewsletterSubscribedDomainEvent @event, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendDiscountCouponEmailCommand(@event.Email, @event.VerificationToken));
    }
}