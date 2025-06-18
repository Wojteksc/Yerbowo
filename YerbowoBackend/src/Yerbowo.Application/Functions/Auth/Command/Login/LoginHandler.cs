namespace Yerbowo.Application.Functions.Auth.Command.Login;

public class LoginHandler(
	IUserRepository userRepository,
    IPasswordManager passwordManager,
    IAuthenticator authenticator,
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<LoginCommand, ResponseToken>
{
    public async Task<ResponseToken> Handle(LoginCommand request, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetAsync(request.Email);

		if (user == null || user.IsRemoved ||
			!passwordManager.Validate(request.Password, user.Password))
		{
			throw new UserInvalidCredentailsException(localizer);
		}

		if(user.VerifiedAt == null || !user.VerifiedAt.HasValue)
        {
			throw new UserRegistrationWasNotConfirmedException(localizer);
        }

		var responseToken = new ResponseToken(authenticator.CreateToken(user.Id, user.Email, user.Role), user.PhotoUrl);
		
		return responseToken;
	}
}