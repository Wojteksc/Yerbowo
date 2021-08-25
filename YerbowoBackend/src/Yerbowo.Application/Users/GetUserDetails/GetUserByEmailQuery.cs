using MediatR;

namespace Yerbowo.Application.Users.GetUserDetails
{
    public class GetUserByEmailQuery : IRequest<UserDetailsDto>
    {
        public string Email { get; }

        public GetUserByEmailQuery(string email)
        {
            Email = email;
        }

    }
}
