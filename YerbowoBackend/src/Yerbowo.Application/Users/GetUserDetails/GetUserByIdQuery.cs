using MediatR;

namespace Yerbowo.Application.Users.GetUserDetails
{
	public class GetUserByIdQuery : IRequest<UserDetailsDto>
	{
		public int UserId { get; }

		public GetUserByIdQuery(int userId)
		{
			UserId = userId;
		}
	}
}
