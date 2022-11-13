using Serilog;

namespace Yerbowo.Functional.Tests.Web.Controllers;

public class AddressesControllerTest : ApiTestBase
{
    private readonly HttpClient _httpClient;
    public AddressesControllerTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
        _httpClient = CreateClient();
    }

    [Fact]
    public async Task GetAddresses_Should_ReturnStatusCodeUnauthorized_When_UserIsAnonymous()
    {
        var response = await _httpClient.GetAsync("/api/users/9999999/addresses");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateAddress_Should_CreateCorrectly()
    {
        var address = new CreateAddressCommand()
        {
            UserId = User.Id,
            Alias = "Warszawa 100",
            FirstName = User.FirstName,
            LastName = User.LastName,
            Place = "Warszawa",
            PostCode = "00-001",
            Street = "test",
            BuildingNumber = "100",
            ApartmentNumber = "",
            Phone = "000-000-000",
            Email = User.Email,
        };

        var createdAddress = await _httpClient.PostAsync<CreateAddressCommand, AddressDetailsDto>
            ($"api/users/{User.Id}/addresses", address);

        createdAddress.Should().BeEquivalentTo(address);
    }

    [Fact]
    public async Task UpdateAddress_Should_UpdateCorrectly()
    {
        var newAddress = new CreateAddressCommand()
        {
            UserId = User.Id,
            Alias = "My home",
            FirstName = User.FirstName,
            LastName = User.LastName,
            Place = "Warszawa",
            PostCode = "00-005",
            Street = "test54",
            BuildingNumber = "12A",
            Phone = "000-000-000",
            Email = User.Email
        };

        var createdAddress = await _httpClient.PostAsync<CreateAddressCommand, AddressDetailsDto>
            ($"/api/users/{User.Id}/addresses", newAddress);

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
            FirstName = User.FirstName,
            LastName = User.LastName,
            PostCode = "00-321"
        };

        var messagePut = await _httpClient.PutAsync($"/api/users/{User.Id}/addresses/{createdAddress.Id}", addressCommand);

        var updatedAddress = await _httpClient.GetAsync<AddressDetailsDto>($"api/users/{User.Id}/addresses/{addressCommand.Id}");

        messagePut.StatusCode.Should().Be(HttpStatusCode.NoContent);
        updatedAddress.Should().BeEquivalentTo(addressCommand);
    }

    [Fact]
    public async Task DeleteAddress_Should_DeleteCorrectly()
    {
        var addressCommand = new CreateAddressCommand()
        {
            UserId = User.Id,
            Alias = "My home",
            FirstName = User.FirstName,
            LastName = User.LastName,
            Place = "Warszawa",
            PostCode = "00-005",
            Street = "test54",
            BuildingNumber = "12A",
            Phone = "000-000-000",
            Email = User.Email
        };

        var createdAddress = await _httpClient.PostAsync<CreateAddressCommand, AddressDetailsDto>
            ($"/api/users/{User.Id}/addresses", addressCommand);

        var messageDelete = await _httpClient.DeleteAsync($"/api/users/{User.Id}/addresses/{createdAddress.Id}");

        var deletedAddress = await _httpClient.GetAsync<AddressDetailsDto>($"api/users/{User.Id}/addresses/{createdAddress.Id}");

        messageDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
        deletedAddress.Should().Be(null);
    }
}