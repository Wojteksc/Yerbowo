namespace Yerbowo.Application.Functions.Auth.Command.Login;

public record LoginCommand : ICommand<ResponseToken>
{
    [Required(ErrorMessage = "Adres e-mail jest wymagany")]
    [EmailAddress(ErrorMessage = "Wprowadź poprawny adres e-mail")]
    public string Email { get; init; }
    [Required(ErrorMessage = "Hasło jest wymagane")]
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string PhotoUrl { get; init; }
    public string Provider { get; init; }
}