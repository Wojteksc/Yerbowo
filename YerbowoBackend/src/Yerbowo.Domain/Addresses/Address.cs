using Ardalis.GuardClauses;
using Yerbowo.Domain.SeedWork;
using Yerbowo.Domain.Users;
using static Ardalis.GuardClauses.Guard;

namespace Yerbowo.Domain.Addresses
{
    public class Address : BaseEntity
    {
        public int UserId { get; protected set; }

        public User User { get; protected set; }
        public string Alias { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string Street { get; protected set; }
        public string BuildingNumber { get; protected set; }
        public string ApartmentNumber { get; protected set; }
        public string Place { get; protected set; }
        public string PostCode { get; protected set; }
        public string Phone { get; protected set; }
        public string Email { get; protected set; }
        public string Nip { get; protected set; }
        public string Company { get; protected set; }

        private Address() {}

        public Address(int userId, string alias, string firstName, string lastName, 
            string street, string buildingNumber, string apartmentNumber, string place, 
            string postCode, string phone, string email, string nip = "", string company = "")
        {
            Against.NegativeOrZero(userId, nameof(userId));
            Against.NullOrEmpty(alias, nameof(alias));
            Against.NullOrEmpty(firstName, nameof(firstName));
            Against.NullOrEmpty(lastName, nameof(lastName));
            Against.NullOrEmpty(street, nameof(street));
            Against.NullOrEmpty(buildingNumber, nameof(buildingNumber));
            Against.NullOrEmpty(place, nameof(place));
            Against.NullOrEmpty(postCode, nameof(postCode));
            Against.NullOrEmpty(phone, nameof(phone));
            Against.NullOrEmpty(email, nameof(email));

            UserId = userId;
            Alias = alias;
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            BuildingNumber = buildingNumber;
            ApartmentNumber = apartmentNumber;
            Place = place;
            PostCode = postCode;
            Phone = phone;
            Email = email;
            Nip = nip;
            Company = company;
        }
    }
}
