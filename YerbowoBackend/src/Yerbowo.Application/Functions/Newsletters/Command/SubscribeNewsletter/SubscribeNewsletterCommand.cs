namespace Yerbowo.Application.Functions.Newsletters.Command.SubscribeNewsletter;

public record SubscribeNewsletterCommand(string Email, string Token) : ICommand { }