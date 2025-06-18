namespace Yerbowo.Domain.Entities.Users;

public class User : AgregateRoot
{
    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }
    public string CompanyName { get; protected set; }
    public string Email { get; protected set; }
    public string Role { get; protected set; }
    public string PhotoUrl { get; protected set; }
    public string Provider { get; protected set; }
    public string Password { get; protected set; }
    public string VerificationToken { get; protected set; }
    public DateTime? VerifiedAt { get; protected set; }

    public List<Address> Addresses { get; protected set; }

    private User() { }

    public User(string firstName, 
        string lastName, 
        string email,
        string role = "user",
        string companyName = null, 
        string photoUrl = null,
        string provider = null)
    {

        Against.NullOrEmpty(firstName, nameof(firstName));
        Against.NullOrEmpty(lastName, nameof(lastName));
        Against.NullOrEmpty(email, nameof(email));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        CompanyName = companyName;
        SetRole(role);
        SetPhotoUrl(photoUrl);
        Provider = provider;
    }
    public void Register()
    {
        RaiseDomainEvent(new UserRegisteredDomainEvent(FirstName, Email, VerificationToken));
    }

    public void SetPassword(string hashedPassword)
    {
        Against.NullOrEmpty(hashedPassword, nameof(hashedPassword));

        Password = hashedPassword;
    }

    public void SetRole(string role)
    {
        Against.NullOrEmpty(role, nameof(role));

        Role = role;
    }

    public void SetPhotoUrl(string photoUrl)
    {
        PhotoUrl = photoUrl;
    }

    public void SetVerificationToken(string verificationToken)
    {
        Against.NullOrEmpty(verificationToken, nameof(verificationToken));

        VerificationToken = verificationToken;
    }

    public void SetVerificationDate(DateTime verifiedAt)
    {
        Against.Default(verifiedAt, nameof(verifiedAt));

        VerifiedAt = verifiedAt;
    }
}