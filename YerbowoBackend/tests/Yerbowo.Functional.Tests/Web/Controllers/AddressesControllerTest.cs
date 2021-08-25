using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Yerbowo.Application.Addresses.ChangeAddresses;
using Yerbowo.Application.Addresses.CreateAddresses;
using Yerbowo.Application.Addresses.GetAddressDetails;
using Yerbowo.Application.Addresses.GetAddresses;
using Yerbowo.Application.Auth.Register;
using Yerbowo.Application.Users.GetUserDetails;
using Yerbowo.Functional.Tests.Web.Extensions;
using Yerbowo.Functional.Tests.Web.Helpers;

namespace Yerbowo.Functional.Tests.Web.Controllers
{
    public class AddressesControllerTest : IClassFixture<WebTestFixture>
    {
        private readonly HttpClient _httpClient;
        public AddressesControllerTest(WebTestFixture factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions());
        }

        [Fact]
        public async Task Get_Addresses_Should_Return_Unauthorized_When_Anonymous_User()
        {
            var response = await _httpClient.GetAsync("/api/users/1/addresses");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Create_Two_Addresses_For_New_User_Should_Return_Two_Addresses()
        {
            var registerCommand = new RegisterCommand()
            {
                FirstName = "FirstNameTest",
                LastName = "LastNameTest",
                CompanyName = "TestCompany",
                Email = "adressesControllerTest@gmail.com",
                ConfirmEmail = "adressesControllerTest@gmail.com",
                Password = "secret",
                ConfirmPassword = "secret"
            };

            UserDetailsDto user = await AuthHelper.SignUp(_httpClient, registerCommand);

            var address1 = new CreateAddressCommand()
            {
                UserId = user.Id,
                Alias = "Warszawa 100",
                FirstName = user.FirstName,
                Lastname = user.LastName,
                Place = "Warszawa",
                PostCode = "00-001",
                Street = "test",
                BuildingNumber = "100",
                ApartmentNumber = "",
                Phone = "000-000-000",
                Email = user.Email,
            };

            var address2 = new CreateAddressCommand()
            {
                UserId = user.Id,
                Alias = "Warszawa 12a/4",
                FirstName = user.FirstName,
                Lastname = user.LastName,
                Place = "Warszawa",
                PostCode = "00-002",
                Street = "test2",
                BuildingNumber = "12A",
                ApartmentNumber = "4",
                Phone = "000-000-000",
                Email = user.Email
            };

            var createdAddress1 = await _httpClient.PostAsync<CreateAddressCommand, AddressDetailsDto>
                ($"api/users/{user.Id}/addresses", address1);

            var createdAddress2 = await _httpClient.PostAsync<CreateAddressCommand, AddressDetailsDto>
                ($"api/users/{user.Id}/addresses", address2);

            var addresses = await _httpClient.GetAsync<List<AddressDto>>($"api/users/{user.Id}/addresses");

            Assert.Equal(2, addresses.Count);

            Assert.Equal(address1.Alias, createdAddress1.Alias);
            Assert.Equal(address1.Email, createdAddress1.Email);
            Assert.Equal(address1.Phone, createdAddress1.Phone);
            Assert.Equal(address1.UserId, createdAddress1.UserId);

            Assert.Equal(address2.Alias, createdAddress2.Alias);
            Assert.Equal(address2.Email, createdAddress2.Email);
            Assert.Equal(address2.Phone, createdAddress2.Phone);
            Assert.Equal(address2.UserId, createdAddress2.UserId);
        }

        [Fact]
        public async Task Delete_Address_Should_Return_Status_Code_204()
        {
            var registerCommand = new RegisterCommand()
            {
                FirstName = "FirstNameTestDelete",
                LastName = "LastNameTestDelete",
                CompanyName = "TestCompanyDelete",
                Email = "adressesControllerDeleteTest@gmail.com",
                ConfirmEmail = "adressesControllerDeleteTest@gmail.com",
                Password = "secret",
                ConfirmPassword = "secret"
            };

            UserDetailsDto user = await AuthHelper.SignUp(_httpClient, registerCommand);

            var newAddress = new CreateAddressCommand()
            {
                UserId = user.Id,
                Alias = "My home",
                FirstName = user.FirstName,
                Lastname = user.LastName,
                Place = "Warszawa",
                PostCode = "00-005",
                Street = "test54",
                BuildingNumber = "12A",
                Phone = "000-000-000",
                Email = user.Email
            };

            var createdAddress = await _httpClient.PostAsync<CreateAddressCommand, AddressDetailsDto>
                ($"/api/users/{user.Id}/addresses", newAddress);

            var responseDeletedAddress = await _httpClient.DeleteAsync($"/api/users/{user.Id}/addresses/{createdAddress.Id}");

            var deletedAddress = await _httpClient.GetAsync<AddressDetailsDto>($"api/users/{user.Id}/addresses/{createdAddress.Id}");

            Assert.Equal(HttpStatusCode.NoContent, responseDeletedAddress.StatusCode);
            Assert.Null(deletedAddress);
        }

        [Fact]
        public async Task Update_Address_Should_Return_Status_Code_204()
        {
            var registerCommand = new RegisterCommand()
            {
                FirstName = "FirstNameTestAddressUpdate",
                LastName = "LastNameTestAddressUpdate",
                CompanyName = "TestCompanyAddressUpdate",
                Email = "adressesControllerUpdateTest@gmail.com",
                ConfirmEmail = "adressesControllerUpdateTest@gmail.com",
                Password = "secret",
                ConfirmPassword = "secret"
            };

            UserDetailsDto user = await AuthHelper.SignUp(_httpClient, registerCommand);

            var newAddress = new CreateAddressCommand()
            {
                UserId = user.Id,
                Alias = "My home",
                FirstName = user.FirstName,
                Lastname = user.LastName,
                Place = "Warszawa",
                PostCode = "00-005",
                Street = "test54",
                BuildingNumber = "12A",
                Phone = "000-000-000",
                Email = user.Email
            };

            var createdAddress = await _httpClient.PostAsync<CreateAddressCommand, AddressDetailsDto>
                ($"/api/users/{user.Id}/addresses", newAddress);

            var addressToUpdate = new ChangeAddressCommand()
            {
                Id = createdAddress.Id,
                Alias = "updated 999",
                Email = "updated@updated999.pl",
                Phone = "123-123-123",
                BuildingNumber = "999A",
                ApartmentNumber = "99",
                Place = "Place updated",
                Street = "Street updated",
                FirstName = user.FirstName,
                Lastname = user.LastName,
                PostCode = "00-321"
            };

            var messagePut = await _httpClient.PutAsync($"/api/users/{user.Id}/addresses/{createdAddress.Id}", addressToUpdate);

            var updatedAddress = await _httpClient.GetAsync<AddressDetailsDto>($"api/users/{user.Id}/addresses/{addressToUpdate.Id}");

            Assert.Equal(HttpStatusCode.NoContent, messagePut.StatusCode);
            Assert.Equal(addressToUpdate.Id, updatedAddress.Id);
            Assert.Equal(addressToUpdate.Alias, updatedAddress.Alias);
            Assert.Equal(addressToUpdate.Email, updatedAddress.Email);
            Assert.Equal(addressToUpdate.Phone, updatedAddress.Phone);
            Assert.Equal(addressToUpdate.BuildingNumber, updatedAddress.BuildingNumber);
            Assert.Equal(addressToUpdate.ApartmentNumber, updatedAddress.ApartmentNumber);
            Assert.Equal(addressToUpdate.Place, updatedAddress.Place);
            Assert.Equal(addressToUpdate.FirstName, updatedAddress.FirstName);
            Assert.Equal(addressToUpdate.Lastname, updatedAddress.LastName);
            Assert.Equal(addressToUpdate.Street, updatedAddress.Street);
            Assert.Equal(addressToUpdate.PostCode, updatedAddress.PostCode);
        }
    }
}
