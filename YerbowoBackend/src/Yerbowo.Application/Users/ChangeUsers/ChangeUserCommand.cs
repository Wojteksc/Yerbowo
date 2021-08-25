using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Yerbowo.Application.Users.ChangeUsers
{
    public class ChangeUserCommand : IRequest
	{
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }

        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Wprowadź prawidłowy adres e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [EmailAddress(ErrorMessage = "Wprowadź prawidłowy adres e-mail")]
        [Compare("Email", ErrorMessage = "Email i potwierdzenie E-Mail nie są identyczne")]
        public string ConfirmEmail { get; set; }

        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Hasło i potwierdzenie hasła nie są identyczne")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(int.MaxValue, ErrorMessage = "Hasło musi mieć conajmniej 6 znaków", MinimumLength = 6)]
        public string CurrentPassword { get; set; }
    }
}
