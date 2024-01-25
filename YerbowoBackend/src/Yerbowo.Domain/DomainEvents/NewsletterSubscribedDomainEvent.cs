namespace Yerbowo.DomainEvents;

public class NewsletterSubscribedDomainEvent : IDomainEvent
{
    public string Email { get; }
    public string VerificationToken { get; }

    public NewsletterSubscribedDomainEvent(string email, string verificationToken)
    {
        Email = email;
        VerificationToken = verificationToken;
    }
}