using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Yerbowo.Application.Auth.Register
{
    public class RegisterCommand : IRequest
    {
        [Required(ErrorMessage = "Imię jest wymagane")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }

        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu e-mail")]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(int.MaxValue, ErrorMessage = "Hasło musi mieć conajmniej 6 znaków", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(int.MaxValue, ErrorMessage = "Hasło musi mieć conajmniej 6 znaków", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "Hasło i potwierdzenie hasła nie są identyczne")]
        public string ConfirmPassword { get; set; }

        public string PhotoUrl { get; set; }

        public string Provider { get; set; }
    }
}
