namespace Yerbowo.Application.Functions.Newsletters.Command.UnsubscribeNewsletter;

public record UnsubscribeNewsletterCommand(string Email, string Token) : ICommand { }