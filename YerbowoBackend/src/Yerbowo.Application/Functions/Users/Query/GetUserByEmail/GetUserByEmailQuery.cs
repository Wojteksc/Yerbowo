namespace Yerbowo.Application.Functions.Users.Query.GetUserByEmail;

public record GetUserByEmailQuery(string Email) : IQuery<UserDetailsDto> { }