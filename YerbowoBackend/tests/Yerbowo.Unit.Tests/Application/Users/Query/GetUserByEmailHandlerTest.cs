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

    private Guid UserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    [Fact]
    public async Task Should_ReturnUserCorrectly()
    {
        var user = new User(UserId, "firstName", "lastName", "emailTest@email.com");

        var userQuery = new GetUserByEmailQuery("emailTest@email.com");

        var expectedUser = new UserDetailsDto()
        {
            Id = UserId,
            FirstName = "firstName",
            LastName = "lastName",
            Email = "emailTest@email.com",
            Role = "user"
        };

        _userRepositoryMock
            .Setup(x => x.GetActiveByEmailAsync(userQuery.Email))
            .ReturnsAsync(user);

        var result = await _handler.Handle(userQuery, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedUser);
    }
}