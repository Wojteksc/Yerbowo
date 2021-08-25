using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Yerbowo.Application.Auth.Login
{
    public class LoginCommand : IRequest<ResponseToken>
    {
        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Wprowadź poprawny adres e-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane")]
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoUrl { get; set; }
        public string Provider { get; set; }
    }
}
