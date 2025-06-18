namespace Yerbowo.Application.Functions.Users.Query.GetUserByEmail;

public class GetUserByEmailHandler(
    IMapper mapper, 
    IUserRepository userRepository) : IQueryHandler<GetUserByEmailQuery, UserDetailsDto>
{
    public async Task<UserDetailsDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(request.Email);

        return mapper.Map<UserDetailsDto>(user);
    }
}