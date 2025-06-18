namespace Yerbowo.Application.Functions.Auth.Command.Register;

public record RegisterCommand : ICommand
{
    [Required(ErrorMessage = "Imię jest wymagane")]
    public string FirstName { get; init; }

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    public string LastName { get; init; }

    public string CompanyName { get; init; }

    [Required(ErrorMessage = "Adres e-mail jest wymagany")]
    [EmailAddress(ErrorMessage = "Niepoprawny format adresu e-mail")]
    public string Email { get; init; }

    [Required(ErrorMessage = "Adres e-mail jest wymagany")]
    [EmailAddress(ErrorMessage = "Niepoprawny format adresu e-mail")]
    public string ConfirmEmail { get; init; }

    [Required(ErrorMessage = "Hasło jest wymagane")]
    [StringLength(int.MaxValue, ErrorMessage = "Hasło musi mieć conajmniej 6 znaków", MinimumLength = 6)]
    public string Password { get; init; }

    [Required(ErrorMessage = "Hasło jest wymagane")]
    [StringLength(int.MaxValue, ErrorMessage = "Hasło musi mieć conajmniej 6 znaków", MinimumLength = 6)]
    [Compare("Password", ErrorMessage = "Hasło i potwierdzenie hasła nie są identyczne")]
    public string ConfirmPassword { get; init; }
}