namespace Yerbowo.Application.Functions.Addresses.Command.ChangeAddresses;

public record ChangeAddressCommand : ICommand, ICommandIdentity
{
    public int Id { get; init; }
    [Required(ErrorMessage = "Alias jest wymagany")]
    public string Alias { get; init; }
    [Required(ErrorMessage = "Imię jest wymagane")]
    public string FirstName { get; init; }
    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    public string LastName { get; init; }
    [Required(ErrorMessage = "Ulica jest wymagana")]
    public string Street { get; init; }
    [Required(ErrorMessage = "Numer budynku jest wymagane")]
    public string BuildingNumber { get; init; }
    public string ApartmentNumber { get; init; }
    [Required(ErrorMessage = "Miejscowość jest wymagana")]
    public string Place { get; init; }
    [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
    public string PostCode { get; init; }
    [Required(ErrorMessage = "Telefon jest wymagany")]
    public string Phone { get; init; }
    [Required(ErrorMessage = "Adres e-mail jest wymagany")]
    public string Email { get; init; }
    public string Nip { get; init; }
    public string Company { get; init; }
}