namespace Yerbowo.Unit.Tests.Application.Users.Query;

public class GetUserByIdHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetUserByIdHandler _handler;

    private Guid UserId = Guid.Parse("00000000-0000-0000-0000-000000000001");


    public GetUserByIdHandlerTest()
    {
        _userRepositoryMock = new();

        _handler = new GetUserByIdHandler(
            _userRepositoryMock.Object,
            AutoMapperConfig.Initialize());
    }

    [Fact]
    public async Task Should_ReturnUserCorrectly()
    {
        var user = new User(UserId, "firstName", "lastName", "email@email.com");

        var userQuery = new GetUserByIdQuery(UserId);

        var userDetailsDto = new UserDetailsDto()
        {
            Id = UserId,
            FirstName = "firstName",
            LastName = "lastName",
            Email = "email@email.com",
            Role = "user"
        };

        _userRepositoryMock
            .Setup(x => x.GetAsync(userQuery.UserId))
            .ReturnsAsync(user);

        var result = await _handler.Handle(userQuery, CancellationToken.None);

        result.Should().BeEquivalentTo(userDetailsDto);
    }
}