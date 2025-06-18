namespace Yerbowo.Application.Functions.Users.Query.GetUserDetails;

public class GetUserByIdHandler(
	IUserRepository userRepository, 
	IMapper mapper) : IQueryHandler<GetUserByIdQuery, UserDetailsDto>
{
    public async Task<UserDetailsDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetAsync(request.UserId);

		return mapper.Map<UserDetailsDto>(user);
	}
}