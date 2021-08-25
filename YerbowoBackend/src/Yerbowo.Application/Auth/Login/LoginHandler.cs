using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Yerbowo.Application.Services;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Application.Auth.Login
{
	internal class LoginHandler : IRequestHandler<LoginCommand, ResponseToken>
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

			if (user == null || user.IsRemoved)
				throw new UnauthorizedAccessException("Konto nie istnieje");

			if (!_passwordValidator.Equals(request.Password, user.PasswordHash, user.PasswordSalt))
				throw new UnauthorizedAccessException($"Niepoprawne dane logowania");

			return new ResponseToken()
			{
				Token = _jwtHandler.CreateToken(user.Id, user.Email, user.Role),
				PhotoUrl = user.PhotoUrl
			};
		}
	}
}
