namespace Yerbowo.Application.Functions.Auth.Command.ConfirmEmail;

public class ConfirmEmailCommand : IRequest
{
    public string Email { get; set; }
    public string Token { get; set; }
}