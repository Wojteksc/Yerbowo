namespace Yerbowo.Application.Functions.Newsletters.Command.InviteNewsletter;

public record InviteNewsletterCommand : ICommand<string>
{
    [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail")]
    public string Email { get; init; }
}