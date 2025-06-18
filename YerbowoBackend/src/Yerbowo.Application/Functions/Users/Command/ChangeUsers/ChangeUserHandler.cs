namespace Yerbowo.Application.Functions.Users.Command.ChangeUsers;

public class ChangeUserHandler(
    IMapper mapper,
    IUserRepository userRepository,
    IPasswordManager passwordManager,
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<ChangeUserCommand>
{
    public async Task Handle(ChangeUserCommand request, CancellationToken cancellationToken)
	{
		var userDb = await userRepository.GetAsync(request.Id) 
            ?? throw new UserNotFoundException(localizer);
        
        if (!passwordManager.Validate(request.CurrentPassword, userDb.Password))
            throw new UserPasswordIsIncorrectException(localizer);

        if (!string.IsNullOrEmpty(request.NewPassword) && !string.IsNullOrEmpty(request.ConfirmPassword))
		{
			userDb.SetPassword(passwordManager.Secure(request.ConfirmPassword));
		}

		mapper.Map(request, userDb);

		await userRepository.UpdateAsync(userDb);
	}
}