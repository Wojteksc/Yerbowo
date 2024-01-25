namespace Yerbowo.Application.Functions.Auth.Command.SocialLogin;

public class SocialLoginHandler : IRequestHandler<SocialLoginCommand, ResponseToken>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IJwtProvider _jwtHandler;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public SocialLoginHandler(IUserRepository userRepository,
        IMapper mapper,
        IJwtProvider jwtHandler,
        IStringLocalizer<SharedResource> localizer)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtHandler = jwtHandler;
        _localizer = localizer;
    }

    public async Task<ResponseToken> Handle(SocialLoginCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Email))
            throw new UnauthorizedAccessException(string.Format(_localizer["ExceptionAccountHasNoEmail"], request.Provider.ToTitle()));

        var user = await _userRepository.GetAsync(request.Email);

        if (IsUserRemoved(user))
            throw new UnauthorizedAccessException(_localizer["ExceptionAccountDoesNotExist"]);

        if (user == null)
        {
            user = _mapper.Map<User>(request);
            user.SetRole("user");
            await _userRepository.AddAsync(user);
        }
        else if(string.IsNullOrEmpty(user.PhotoUrl))
        {
            user.SetPhotoUrl(request.PhotoUrl);
            await _userRepository.UpdateAsync(user);
        }

        return new ResponseToken()
        {
            Token = _jwtHandler.CreateToken(user.Id, user.Email, user.Role),
            PhotoUrl = user.PhotoUrl
        };
    }

    private static bool IsUserRemoved(User user)
    {
        return user != null && user.IsRemoved;
    }
}