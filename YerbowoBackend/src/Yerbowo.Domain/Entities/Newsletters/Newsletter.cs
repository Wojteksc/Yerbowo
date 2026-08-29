namespace Yerbowo.Domain.Entities.Newsletters;

public class Newsletter : AgregateRoot
{
    public string Email { get; protected set; }
    public string VerificationToken { get; protected set; }
    public DateTime? VerifiedAt { get; protected set; }
    
    protected Newsletter() { }

    private Newsletter(Guid id, string email, string verificationToken)
    {
        Against.Default(id, nameof(id));
        Against.NullOrEmpty(email, nameof(email));
        
        Id = id;
        Email = email;
        SetToken(verificationToken);
    }

    public static Newsletter Create(Guid id, string email, string verificationToken)
    {
        return new Newsletter(id, email, verificationToken);
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

    public void SetToken(string token)
    {
        Against.NullOrEmpty(token, nameof(token));
        VerificationToken = token;
    }
}