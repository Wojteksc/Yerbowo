namespace Yerbowo.Application.Functions.Auth.Command.ConfirmEmail;

public class ConfirmRegistrationEmailHandler(
    IUserRepository userRepository, 
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<ConfirmRegistrationEmailCommand>
{
    public async Task Handle(ConfirmRegistrationEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(request.Email) 
            ?? throw new UserNotFoundException(localizer);
        
        if (user.VerificationToken != request.Token)
        {
            throw new InvalidTokenException(localizer);
        }

        if (user.VerifiedAt != null)
        {
            throw new EmailWasAlreadyConfirmedException(localizer);
        }

        user.SetVerificationDate(DateTime.UtcNow);
        await userRepository.UpdateAsync(user);
    }
}