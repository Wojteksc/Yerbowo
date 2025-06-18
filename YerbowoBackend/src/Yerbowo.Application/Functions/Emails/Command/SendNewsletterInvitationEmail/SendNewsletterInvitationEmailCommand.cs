namespace Yerbowo.Application.Functions.Emails.Command.SendNewsletterInvitationEmail;

public record SendNewsletterInvitationEmailCommand(string Email, string VerificationToken) : ICommand { }