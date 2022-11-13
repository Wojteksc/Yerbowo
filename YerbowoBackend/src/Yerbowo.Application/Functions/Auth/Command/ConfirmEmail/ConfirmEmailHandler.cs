namespace Yerbowo.Application.Functions.Auth.Command.ConfirmEmail;

public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand>
{
    private readonly IUserRepository _userRepository;

    public ConfirmEmailHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(request.Email);
        if (user == null || user.Email != request.Email || user.VerificationToken != request.Token)
        {
            throw new Exception("Nieprawidłowe żądanie potwierdzenia adresu e-mail.");
        }
        
        if(user.VerifiedAt != null)
        {
            throw new Exception("Adres e-mail był już potwierdzony.");
        }

        user.SetVerificationDate(DateTime.UtcNow);
        await _userRepository.SaveAllAsync();

        return Unit.Value;
    }
}