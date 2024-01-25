namespace Yerbowo.DomainEvents;

public class UserRegisteredDomainEvent : IDomainEvent
{
    public string FirstName { get; }
    public string Email { get; }
    public string VeriicationToken { get; }

    public UserRegisteredDomainEvent(string firstName, string email, string veryficationToken)
    {
        FirstName = firstName;
        Email = email;
        VeriicationToken = veryficationToken;
    }
}