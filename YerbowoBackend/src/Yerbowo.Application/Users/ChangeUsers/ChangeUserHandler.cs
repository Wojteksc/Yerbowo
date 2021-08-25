using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Yerbowo.Application.Services;
using Yerbowo.Domain.Users;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Application.Users.ChangeUsers
{
	internal class ChangeUserHandler : IRequestHandler<ChangeUserCommand>
	{
		private readonly IMapper _mapper;
		private readonly IUserRepository _userRepository;
		private readonly IPasswordValidator _passwordValidator;

		public ChangeUserHandler(IMapper mapper,
			IUserRepository userRepository,
			IPasswordValidator passwordValidator)
		{
			_mapper = mapper;
			_userRepository = userRepository;
			_passwordValidator = passwordValidator;
		}

		public async Task<Unit> Handle(ChangeUserCommand request, CancellationToken cancellationToken)
		{
			if (!await _userRepository.ExistsAsync(request.Id))
				throw new Exception("Użytkownik nie istnieje");

			var userFromRepo = await _userRepository.GetAsync(request.Id);

			if (userFromRepo == null)
				throw new Exception("Nie znaleziono użytkownika.");

			ValidateUpdatedData(request, userFromRepo);

			if (!string.IsNullOrEmpty(request.NewPassword) && !string.IsNullOrEmpty(request.ConfirmPassword))
			{
				userFromRepo.SetPassword(request.ConfirmPassword);
			}

			_mapper.Map(request, userFromRepo);

			if(!await _userRepository.SaveAllAsync())
				throw new Exception($"Aktualizacja użytkownika nie powodła się");

			return Unit.Value;
		}

		private void ValidateUpdatedData(ChangeUserCommand command, User userFromRepo)
		{
			if (!_passwordValidator.Equals(command.CurrentPassword, userFromRepo.PasswordHash, userFromRepo.PasswordSalt))
			{
				throw new Exception("Podane hasło jest nieprawidłowe.");
			}
			else if (!string.IsNullOrEmpty(command.NewPassword) && command.NewPassword.Length < 6)
			{
				throw new Exception("Nowe hasło musi zawierać co najmniej 6 znaków.");
			}
			else if (!string.IsNullOrEmpty(command.ConfirmPassword) && command.ConfirmPassword.Length < 6)
			{
				throw new Exception("Powtórz hasło musi zawierać co najmniej 6 znaków.");
			}
		}
	}
}
