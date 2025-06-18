namespace Yerbowo.Application.Functions.Auth.Command.SocialLogin;

public class SocialLoginHandler(
    IUserRepository userRepository,
    IMapper mapper,
    IAuthenticator authenticator,
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<SocialLoginCommand, ResponseToken>
{
    public async Task<ResponseToken> Handle(SocialLoginCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Email))
            throw new UserHasNoEmailException(localizer, request.Provider.ToTitle());

        var user = await userRepository.GetAsync(request.Email);

        if (IsUserRemoved(user))
            throw new UserNotFoundException(localizer);

        if (user == null)
        {
            user = mapper.Map<User>(request);
            user.SetRole("user");
            await userRepository.AddAsync(user);
        }
        else if(string.IsNullOrEmpty(user.PhotoUrl))
        {
            user.SetPhotoUrl(request.PhotoUrl);
            await userRepository.UpdateAsync(user);
        }

        var responseToken = new ResponseToken(authenticator.CreateToken(user.Id, user.Email, user.Role), user.PhotoUrl);

        return responseToken;
    }

    private static bool IsUserRemoved(User user) 
        => user != null && user.IsRemoved;
}