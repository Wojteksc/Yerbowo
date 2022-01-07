using FluentAssertions;
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
        public async Task GetAddresses_Should_ReturnStatusCodeUnauthorized_When_UserIsAnonymous()
        {
            var response = await _httpClient.GetAsync("/api/users/1/addresses");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CreateAddress_Should_CreateCorrectly()
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

            var address = new CreateAddressCommand()
            {
                UserId = user.Id,
                Alias = "Warszawa 100",
                FirstName = user.FirstName,
                LastName = user.LastName,
                Place = "Warszawa",
                PostCode = "00-001",
                Street = "test",
                BuildingNumber = "100",
                ApartmentNumber = "",
                Phone = "000-000-000",
                Email = user.Email,
            };

            var createdAddress = await _httpClient.PostAsync<CreateAddressCommand, AddressDetailsDto>
                ($"api/users/{user.Id}/addresses", address);

            createdAddress.Should().BeEquivalentTo(address);
        }

        [Fact]
        public async Task UpdateAddress_Should_UpdateCorrectly()
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
                LastName = user.LastName,
                Place = "Warszawa",
                PostCode = "00-005",
                Street = "test54",
                BuildingNumber = "12A",
                Phone = "000-000-000",
                Email = user.Email
            };

            var createdAddress = await _httpClient.PostAsync<CreateAddressCommand, AddressDetailsDto>
                ($"/api/users/{user.Id}/addresses", newAddress);

            var addressCommand = new ChangeAddressCommand()
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
                LastName = user.LastName,
                PostCode = "00-321"
            };

            var messagePut = await _httpClient.PutAsync($"/api/users/{user.Id}/addresses/{createdAddress.Id}", addressCommand);

            var updatedAddress = await _httpClient.GetAsync<AddressDetailsDto>($"api/users/{user.Id}/addresses/{addressCommand.Id}");

            messagePut.StatusCode.Should().Be(HttpStatusCode.NoContent);
            updatedAddress.Should().BeEquivalentTo(addressCommand);
        }

        [Fact]
        public async Task DeleteAddress_Should_DeleteCorrectly()
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

            var addressCommand = new CreateAddressCommand()
            {
                UserId = user.Id,
                Alias = "My home",
                FirstName = user.FirstName,
                LastName = user.LastName,
                Place = "Warszawa",
                PostCode = "00-005",
                Street = "test54",
                BuildingNumber = "12A",
                Phone = "000-000-000",
                Email = user.Email
            };

            var createdAddress = await _httpClient.PostAsync<CreateAddressCommand, AddressDetailsDto>
                ($"/api/users/{user.Id}/addresses", addressCommand);

            var messageDelete = await _httpClient.DeleteAsync($"/api/users/{user.Id}/addresses/{createdAddress.Id}");

            var deletedAddress = await _httpClient.GetAsync<AddressDetailsDto>($"api/users/{user.Id}/addresses/{createdAddress.Id}");

            messageDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
            deletedAddress.Should().Be(null);
        }
    }
}