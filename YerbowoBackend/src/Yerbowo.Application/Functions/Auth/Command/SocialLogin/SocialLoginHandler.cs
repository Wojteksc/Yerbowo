namespace Yerbowo.Application.Functions.Auth.Command.SocialLogin;

public class SocialLoginHandler(
    IUserRepository userRepository,
    IMapper mapper,
    ITokenGenerator authenticator,
    IStringLocalizer<SharedResource> localizer,
    IIdGenerator idGenerator) : ICommandHandler<SocialLoginCommand, ResponseToken>
{
    public async Task<ResponseToken> Handle(SocialLoginCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Email))
            throw new UserHasNoEmailException(localizer, request.Provider.ToTitle());

        var user = await userRepository.GetByEmailAsync(request.Email);

        if (IsUserRemoved(user))
            throw new UserNotFoundException(localizer);

        if (user == null)
        {
            var newUser = new User(
                idGenerator.Generate(), 
                request.FirstName, 
                request.LastName, 
                request.Email,
                role: "user",
                companyName: null,
                request.PhotoUrl, 
                request.Provider);
            await userRepository.AddAsync(newUser);
            return new ResponseToken(authenticator.CreateToken(newUser.Id, newUser.Email, newUser.Role), newUser.PhotoUrl);
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