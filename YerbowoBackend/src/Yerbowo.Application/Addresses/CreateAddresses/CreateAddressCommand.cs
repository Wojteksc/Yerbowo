using MediatR;
using System.ComponentModel.DataAnnotations;
using Yerbowo.Application.Addresses.GetAddressDetails;

namespace Yerbowo.Application.Addresses.CreateAddresses
{
    public class CreateAddressCommand : IRequest<AddressDetailsDto>
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Alias jest wymagany")]
        public string Alias { get; set; }
        [Required(ErrorMessage = "Imię jest wymagane")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Ulica jest wymagana")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Numer budynku jest wymagane")]
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
        [Required(ErrorMessage = "Miejscowość jest wymagana")]
        public string Place { get; set; }
        [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
        public string PostCode { get; set; }
        [Required(ErrorMessage = "Telefon jest wymagany")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        public string Email { get; set; }
        public string Nip { get; set; }
        public string Company { get; set; }
    }
}
