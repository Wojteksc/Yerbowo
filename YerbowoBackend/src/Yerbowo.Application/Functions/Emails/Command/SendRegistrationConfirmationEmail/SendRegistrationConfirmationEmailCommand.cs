namespace Yerbowo.Application.Functions.Emails.Command.SendRegistrationConfirmationEmail;

public class SendRegistrationConfirmationEmailCommand : IRequest
{
    public string FirstName { get; }
    public string Email { get; }
    public string VerificationToken { get; }

    public SendRegistrationConfirmationEmailCommand(string firstName, string email, string verificationToken)
    {
        FirstName = firstName;
        Email = email;
        VerificationToken = verificationToken;
    }
}