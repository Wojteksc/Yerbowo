namespace Yerbowo.Domain.Primitives;

[JsonDerivedType(typeof(NewsletterInvitedDomainEvent))]
[JsonDerivedType(typeof(NewsletterSubscribedDomainEvent))]
[JsonDerivedType(typeof(UserRegisteredDomainEvent))]
public interface IDomainEvent : INotification 
{ 
}