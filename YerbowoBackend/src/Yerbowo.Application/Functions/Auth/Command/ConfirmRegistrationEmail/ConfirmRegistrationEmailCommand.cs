namespace Yerbowo.Application.Functions.Auth.Command.ConfirmEmail;

public class ConfirmRegistrationEmailCommand : IRequest
{
    public string Email { get; set; }
    public string Token { get; set; }
}