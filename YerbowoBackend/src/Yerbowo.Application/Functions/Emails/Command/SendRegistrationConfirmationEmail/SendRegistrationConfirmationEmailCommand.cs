namespace Yerbowo.Application.Functions.Emails.Command.SendRegistrationConfirmationEmail;

public record SendRegistrationConfirmationEmailCommand(string FirstName, string Email, string VerificationToken) 
    : ICommand { }