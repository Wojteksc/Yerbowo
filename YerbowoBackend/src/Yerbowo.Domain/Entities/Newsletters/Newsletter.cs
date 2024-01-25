namespace Yerbowo.Domain.Entities.Newsletters;

public class Newsletter : AgregateRoot
{
    public string Email { get; protected set; }
    public string VerificationToken { get; protected set; }
    public DateTime? VerifiedAt { get; protected set; }
    
    protected Newsletter() { }

    private Newsletter(string email, string verificationToken)
    {
        SetEmail(email);
        SetVerificationToken(verificationToken);
    }

    public static Newsletter Create(string email, string verificationToken)
    {
        return new Newsletter(email, verificationToken);
    }

    public void Invite()
    {
        RaiseDomainEvent(new NewsletterInvitedDomainEvent(Email, VerificationToken));
    }

    public void Subscribe()
    {
        VerifiedAt = DateTime.UtcNow;

        RaiseDomainEvent(new NewsletterSubscribedDomainEvent(Email, VerificationToken));
    }

    public void Unsubscribe()
    {
        VerifiedAt = null;
    }

    public bool IsSubscribed()
    {
        return VerifiedAt != null;
    }

    private void SetEmail(string email)
    {
        Against.NullOrEmpty(email, nameof(email));

        Email = email;
    }

    private void SetVerificationToken(string verificationToken)
    {
        Against.NullOrEmpty(verificationToken, nameof(verificationToken));

        VerificationToken = verificationToken;
    }
}