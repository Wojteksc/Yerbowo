namespace Yerbowo.Application.Functions.Newsletters.Command.UnsubscribeNewsletter;

public class UnsubscribeNewsletterCommand : IRequest
{
    public string Email { get; set; }
    public string Token { get; set; }
}