using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Application.Users.GetUserDetails
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, UserDetailsDto>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetUserByEmailHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserDetailsDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(request.Email);

            return _mapper.Map<UserDetailsDto>(user);
        }
    }
}
