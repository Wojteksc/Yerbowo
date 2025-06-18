namespace Yerbowo.DomainEvents;

public class UserRegisteredDomainEvent : IDomainEvent
{
    public string FirstName { get; }
    public string Email { get; }
    public string VerificationToken { get; }

    public UserRegisteredDomainEvent(string firstName, string email, string verificationToken)
    {
        FirstName = firstName;
        Email = email;
        VerificationToken = verificationToken;
    }
}