namespace Yerbowo.Application.Functions.Users.Command.ChangeUsers;

public record ChangeUserCommand : ICommand, ICommandIdentity
{
    public int Id { get; init; }

    [Required(ErrorMessage = "Imię jest wymagane")]
    public string FirstName { get; init; }

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    public string LastName { get; init; }

    public string CompanyName { get; init; }

    [Required(ErrorMessage = "Adres e-mail jest wymagany")]
    [EmailAddress(ErrorMessage = "Wprowadź prawidłowy adres e-mail")]
    public string Email { get; init; }

    [Required(ErrorMessage = "Pole jest wymagane")]
    [EmailAddress(ErrorMessage = "Wprowadź prawidłowy adres e-mail")]
    [Compare("Email", ErrorMessage = "Email i potwierdzenie E-Mail nie są identyczne")]
    public string ConfirmEmail { get; init; }

    [StringLength(int.MaxValue, ErrorMessage = "Hasło musi mieć conajmniej 6 znaków", MinimumLength = 6)]
    public string NewPassword { get; init; }

    [Compare("NewPassword", ErrorMessage = "Hasło i potwierdzenie hasła nie są identyczne")]
    [StringLength(int.MaxValue, ErrorMessage = "Hasło musi mieć conajmniej 6 znaków", MinimumLength = 6)]
    public string ConfirmPassword { get; init; }

    [Required]
    [StringLength(int.MaxValue, ErrorMessage = "Hasło musi mieć conajmniej 6 znaków", MinimumLength = 6)]
    public string CurrentPassword { get; init; }
}