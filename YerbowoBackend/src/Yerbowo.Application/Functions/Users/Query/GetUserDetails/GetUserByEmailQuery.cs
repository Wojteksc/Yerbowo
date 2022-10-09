namespace Yerbowo.Application.Functions.Users.Query.GetUserDetails;

public class GetUserByEmailQuery : IRequest<UserDetailsDto>
{
    public string Email { get; }

    public GetUserByEmailQuery(string email)
    {
        Email = email;
    }
}