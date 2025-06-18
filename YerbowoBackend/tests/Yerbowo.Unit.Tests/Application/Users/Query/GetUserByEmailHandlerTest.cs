namespace Yerbowo.Unit.Tests.Application.Users.Query;

public class GetUserByEmailHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetUserByEmailHandler _handler;

    public GetUserByEmailHandlerTest()
    {
        _userRepositoryMock = new();

        _handler = new GetUserByEmailHandler(
            AutoMapperConfig.Initialize(),
            _userRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_ReturnUserCorrectly()
    {
        var user = new User("firstName", "lastName", "email@email.com");

        var userQuery = new GetUserByEmailQuery("emailTest@email.pl");

        var userDetailsDto = new UserDetailsDto()
        {
            FirstName = "firstName",
            LastName = "lastName",
            Email = "email@email.com",
            Role = "user"
        };

        _userRepositoryMock
            .Setup(x => x.GetAsync(userQuery.Email))
            .ReturnsAsync(user);

        var result = await _handler.Handle(userQuery, CancellationToken.None);

        result.Should().BeEquivalentTo(userDetailsDto);
    }
}