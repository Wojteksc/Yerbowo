namespace Yerbowo.Application.Functions.Users.Query.GetUserDetails;

public class GetUserByIdQuery : IRequest<UserDetailsDto>
{
	public int UserId { get; }

	public GetUserByIdQuery(int userId)
	{
		UserId = userId;
	}
}