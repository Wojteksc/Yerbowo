namespace Yerbowo.Application.Functions.Users.Query.GetUserDetails;

public record GetUserByIdQuery(int UserId) : IQuery<UserDetailsDto> { }