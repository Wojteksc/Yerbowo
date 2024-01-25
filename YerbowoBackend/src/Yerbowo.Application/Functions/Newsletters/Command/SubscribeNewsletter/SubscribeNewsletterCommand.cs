namespace Yerbowo.Application.Functions.Newsletters.Command.SubscribeNewsletter;

public class SubscribeNewsletterCommand : IRequest
{
    public string Email { get; set; }
    public string Token { get; set; }
}