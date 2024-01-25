namespace Yerbowo.Application.Functions.Newsletters.Command.InviteNewsletter;

public class InviteNewsletterCommand : IRequest<string>
{
    [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail")]
    public string Email { get; set; }
}