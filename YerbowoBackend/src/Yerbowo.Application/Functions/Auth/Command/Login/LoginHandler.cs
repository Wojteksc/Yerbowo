namespace Yerbowo.Application.Functions.Auth.Command.Login;

public class LoginHandler : IRequestHandler<LoginCommand, ResponseToken>
{
	private readonly IUserRepository _userRepository;
	private readonly IPasswordValidator _passwordValidator;
	private readonly IJwtHandler _jwtHandler;
	private readonly IStringLocalizer<SharedResource> _localizer;

    public LoginHandler(IUserRepository userRepository,
        IPasswordValidator passwordValidator,
        IJwtHandler jwtHandler,
        IStringLocalizer<SharedResource> localizer)
    {
        _userRepository = userRepository;
        _passwordValidator = passwordValidator;
        _jwtHandler = jwtHandler;
        _localizer = localizer;
    }

    public async Task<ResponseToken> Handle(LoginCommand request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetAsync(request.Email);

		if (user == null || user.IsRemoved ||
			!_passwordValidator.Equals(request.Password, user.PasswordHash, user.PasswordSalt))
		{
			throw new UnauthorizedAccessException(_localizer["ExceptionInvalidLoginDetails"]);
		}

		if(user.VerifiedAt == null || !user.VerifiedAt.HasValue)
        {
			throw new UnauthorizedAccessException(_localizer["ExceptionAccountRegistrationHasNotBeenConfirmed"]);
        }

		return new ResponseToken()
		{
			Token = _jwtHandler.CreateToken(user.Id, user.Email, user.Role),
			PhotoUrl = user.PhotoUrl
		};
	}
}