namespace Yerbowo.Unit.Tests.Application.Users.Query;

public class GetUserByIdHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetUserByIdHandler _handler;

    public GetUserByIdHandlerTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();

        _handler = new GetUserByIdHandler(
            _userRepositoryMock.Object,
            AutoMapperConfig.Initialize());
    }

    [Fact]
    public async Task Should_ReturnUserCorrectly()
    {
        var userId = 1;
        var user = new User("firstName", "lastName", "email@email.com", "password");

        var userQuery = new GetUserByIdQuery(1);

        var userDetailsDto = new UserDetailsDto()
        {
            Id = userId,
            FirstName = "firstName",
            LastName = "lastName",
            Email = "email@email.com",
            Role = "user"
        };

        typeof(User).GetProperty(nameof(User.Id)).SetValue(user, userId, null);

        _userRepositoryMock.Setup(x => x.GetAsync(userQuery.UserId))
            .ReturnsAsync(user);

        var result = await _handler.Handle(userQuery, CancellationToken.None);

        result.Should().BeEquivalentTo(userDetailsDto);
    }
}