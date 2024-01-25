namespace Yerbowo.Application.Functions.Auth.Command.ConfirmEmail;

public class ConfirmRegistrationEmailHandler : IRequestHandler<ConfirmRegistrationEmailCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public ConfirmRegistrationEmailHandler(IUserRepository userRepository, IStringLocalizer<SharedResource> localizer)
    {
        _userRepository = userRepository;
        _localizer = localizer;
    }

    public async Task Handle(ConfirmRegistrationEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(request.Email);
        if (user == null || user.VerificationToken != request.Token)
        {
            throw new ArgumentException(_localizer["ResponseBadRequest"]);
        }
        
        if(user.VerifiedAt != null)
        {
            throw new Exception(_localizer["ExceptionEmailWasConfirmed"]);
        }

        user.SetVerificationDate(DateTime.UtcNow);
        await _userRepository.UpdateAsync(user);
    }
}