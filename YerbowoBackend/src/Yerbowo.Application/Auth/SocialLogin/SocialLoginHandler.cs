using AutoMapper;
using MediatR;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Yerbowo.Application.Services;
using Yerbowo.Domain.Extensions;
using Yerbowo.Domain.Users;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Application.Auth.SocialLogin
{
    internal class SocialLoginHandler : IRequestHandler<SocialLoginCommand, ResponseToken>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtHandler _jwtHandler;

        public SocialLoginHandler(IUserRepository userRepository,
            IMapper mapper,
            IJwtHandler jwtHandler)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
        }

        public async Task<ResponseToken> Handle(SocialLoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email))
                throw new UnauthorizedAccessException($"Na Twoim koncie {(request.Provider.ToTitle())} nie jest zapisany adres e-mail");

            var user = await _userRepository.GetAsync(request.Email);

            if (IsUserRemoved(user))
                throw new UnauthorizedAccessException($"Konto nie istnieje");

            if (user == null)
            {
                user = _mapper.Map<User>(request);
                user.SetRole("user");
                await _userRepository.AddAsync(user);
            }
            else if(string.IsNullOrEmpty(user.PhotoUrl))
            {
                user.SetPhotoUrl(request.PhotoUrl);
                await _userRepository.SaveAllAsync();
            }

            return new ResponseToken()
            {
                Token = _jwtHandler.CreateToken(user.Id, user.Email, user.Role),
                PhotoUrl = user.PhotoUrl
            };
        }

        private bool IsUserRemoved(User user)
        {
            return user != null && user.IsRemoved;
        }
    }
}
