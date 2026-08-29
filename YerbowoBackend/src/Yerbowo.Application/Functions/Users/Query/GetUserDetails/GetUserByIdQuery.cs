namespace Yerbowo.Application.Functions.Users.Query.GetUserDetails;

public record GetUserByIdQuery(Guid UserId) : IQuery<UserDetailsDto> { }