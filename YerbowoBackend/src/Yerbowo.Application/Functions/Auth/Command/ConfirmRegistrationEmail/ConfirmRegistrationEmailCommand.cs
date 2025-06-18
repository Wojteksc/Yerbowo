namespace Yerbowo.Application.Functions.Auth.Command.ConfirmEmail;

public record ConfirmRegistrationEmailCommand(string Email, string Token) : ICommand { }