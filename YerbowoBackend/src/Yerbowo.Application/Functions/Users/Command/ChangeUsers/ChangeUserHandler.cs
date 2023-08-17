namespace Yerbowo.Application.Functions.Users.Command.ChangeUsers;

public class ChangeUserHandler : IRequestHandler<ChangeUserCommand>
{
	private readonly IMapper _mapper;
	private readonly IUserRepository _userRepository;
	private readonly IPasswordValidator _passwordValidator;
	private readonly IStringLocalizer<SharedResource> _localizer;

    public ChangeUserHandler(IMapper mapper,
        IUserRepository userRepository,
        IPasswordValidator passwordValidator,
        IStringLocalizer<SharedResource> localizer)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _passwordValidator = passwordValidator;
        _localizer = localizer;
    }

    public async Task<Unit> Handle(ChangeUserCommand request, CancellationToken cancellationToken)
	{
		var userDb = await _userRepository.GetAsync(request.Id);

		if (userDb == null)
            throw new Exception(_localizer["ExceptionUserNotFound"]);

        if (!_passwordValidator.Equals(request.CurrentPassword, userDb.PasswordHash, userDb.PasswordSalt))
            throw new Exception(_localizer["ExceptionPasswordIsIncorrect"]);

        if (!string.IsNullOrEmpty(request.NewPassword) && !string.IsNullOrEmpty(request.ConfirmPassword))
		{
			userDb.SetPassword(request.ConfirmPassword);
		}

		_mapper.Map(request, userDb);

		await _userRepository.UpdateAsync(userDb);

		return Unit.Value;
	}
}