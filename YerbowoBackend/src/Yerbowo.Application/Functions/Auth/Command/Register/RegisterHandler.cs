namespace Yerbowo.Application.Functions.Auth.Command.Register;

public class RegisterHandler
    (IUserRepository userRepository,
    IMapper mapper,
    IStringLocalizer<SharedResource> localizer,
    IWebEncoder webEncoder,
    IPasswordManager passwordManager) : ICommandHandler<RegisterCommand>
{
    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsAsync(request.Email))
            throw new EmailIsAlreadyInUseException(localizer);

        string token = webEncoder.Base64UrlEncodeGuid();

        var user = mapper.Map<User>(request);
        user.SetPassword(passwordManager.Secure(request.Password));
        user.SetRole("user");
        user.SetVerificationToken(token);
        
        user.Register();

        await userRepository.AddAsync(user);
    }
}