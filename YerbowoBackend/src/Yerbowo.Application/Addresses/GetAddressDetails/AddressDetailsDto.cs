namespace Yerbowo.Application.Addresses.GetAddressDetails
{
    public class AddressDetailsDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Alias { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string Place { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Nip { get; set; }
        public string Company { get; set; }
    }
}
