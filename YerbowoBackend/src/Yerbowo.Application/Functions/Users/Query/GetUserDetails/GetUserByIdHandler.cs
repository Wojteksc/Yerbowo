using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Application.Functions.Users.Query.GetUserDetails;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDetailsDto>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<UserDetailsDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetAsync(request.UserId);

		return _mapper.Map<UserDetailsDto>(user);
	}
}