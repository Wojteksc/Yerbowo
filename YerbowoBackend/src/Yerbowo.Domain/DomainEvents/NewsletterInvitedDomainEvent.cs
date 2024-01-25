namespace Yerbowo.DomainEvents;

public class NewsletterInvitedDomainEvent : IDomainEvent
{
    public string Email { get; }
    public string VerificationToken { get; }

    public NewsletterInvitedDomainEvent(string email, string verificationToken)
    {
        Email = email;
        VerificationToken = verificationToken;
    }
}