using Yerbowo.Application.Services.Jwt;
using Yerbowo.Application.Services.PasswordValidator;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Application.Functions.Auth.Command.Login;

public class LoginHandler : IRequestHandler<LoginCommand, ResponseToken>
{
	private readonly IUserRepository _userRepository;
	private readonly IPasswordValidator _passwordValidator;
	private readonly IJwtHandler _jwtHandler;

	public LoginHandler(IUserRepository userRepository,
		IPasswordValidator passwordValidator,
		IJwtHandler jwtHandler)
	{
		_userRepository = userRepository;
		_passwordValidator = passwordValidator;
		_jwtHandler = jwtHandler;
	}

	public async Task<ResponseToken> Handle(LoginCommand request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetAsync(request.Email);

		if (user == null || user.IsRemoved ||
			!_passwordValidator.Equals(request.Password, user.PasswordHash, user.PasswordSalt))
			throw new UnauthorizedAccessException("Niepoprawne dane logowania");

		if(user.VerifiedAt == null || !user.VerifiedAt.HasValue)
        {
			throw new UnauthorizedAccessException("Rejestracja w sklepie nie została potwierdzona. Odbierz pocztę i kliknij w link potwierdzający.");
        }

		return new ResponseToken()
		{
			Token = _jwtHandler.CreateToken(user.Id, user.Email, user.Role),
			PhotoUrl = user.PhotoUrl
		};
	}
}