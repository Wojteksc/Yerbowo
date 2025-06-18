namespace Yerbowo.Application.Functions.Newsletters.Events;

[ExcludeFromCodeCoverage]
public class NewsletterSubscribedEventHandler(IRequestDispatcher dispatcher) 
    : INotificationHandler<NewsletterSubscribedDomainEvent>
{
    public async Task Handle(NewsletterSubscribedDomainEvent @event, CancellationToken cancellationToken)
    {
        await dispatcher.ExecuteCommand(new SendDiscountCouponEmailCommand(@event.Email, @event.VerificationToken));
    }
}