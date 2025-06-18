namespace Yerbowo.Application.Functions.Newsletters.Events;

[ExcludeFromCodeCoverage]
public class NewsletterInvitedEventHandler(IRequestDispatcher dispatcher) 
    : INotificationHandler<NewsletterInvitedDomainEvent>
{
    public async Task Handle(NewsletterInvitedDomainEvent @event, CancellationToken cancellationToken)
    {
        await dispatcher.ExecuteCommand(new SendNewsletterInvitationEmailCommand(@event.Email, @event.VerificationToken));
    }
}