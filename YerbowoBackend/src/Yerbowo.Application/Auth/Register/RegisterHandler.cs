using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Yerbowo.Application.Services;
using Yerbowo.Domain.Users;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Application.Auth.Register
{
    internal class RegisterHandler : IRequestHandler<RegisterCommand>
	{
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RegisterHandler(IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

		public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
		{
            if (await _userRepository.ExistsAsync(request.Email))
                throw new Exception($"Nazwa użytkownika jest zajęta");

            var user = _mapper.Map<User>(request);
            user.SetPassword(request.Password);
            user.SetRole("user");

            await _userRepository.AddAsync(user);

            return Unit.Value;
        }
	}
}
