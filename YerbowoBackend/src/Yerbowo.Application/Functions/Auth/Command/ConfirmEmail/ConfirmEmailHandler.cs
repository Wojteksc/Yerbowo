namespace Yerbowo.Application.Functions.Auth.Command.ConfirmEmail;

public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public ConfirmEmailHandler(IUserRepository userRepository, IStringLocalizer<SharedResource> localizer)
    {
        _userRepository = userRepository;
        _localizer = localizer;
    }

    public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(request.Email);
        if (user == null || user.VerificationToken != request.Token)
        {
            throw new Exception(_localizer["ResponseBadRequest"]);
        }
        
        if(user.VerifiedAt != null)
        {
            throw new Exception(_localizer["ExceptionEmailWasConfirmed"]);
        }

        user.SetVerificationDate(DateTime.UtcNow);
        await _userRepository.UpdateAsync(user);

        return Unit.Value;
    }
}