namespace Yerbowo.Application.Functions.Addresses.Query.GetAddressDetails;

public record AddressDetailsDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Alias { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Street { get; init; }
    public string BuildingNumber { get; init; }
    public string ApartmentNumber { get; init; }
    public string Place { get; init; }
    public string PostCode { get; init; }
    public string Phone { get; init; }
    public string Email { get; init; }
    public string Nip { get; init; }
    public string Company { get; init; }
}