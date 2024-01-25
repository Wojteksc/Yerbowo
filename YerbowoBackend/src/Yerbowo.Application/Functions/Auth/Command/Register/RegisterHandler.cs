namespace Yerbowo.Application.Functions.Auth.Command.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly IWebEncoder _webEncoder;

    public RegisterHandler(IUserRepository userRepository,
        IMapper mapper,
        IMediator mediator,
        IStringLocalizer<SharedResource> localizer,
        IWebEncoder webEncoder)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _mediator = mediator;
        _localizer = localizer;
        _webEncoder = webEncoder;
    }

    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsAsync(request.Email))
            throw new Exception(_localizer["ExceptionEmailIsAlreadyInUse"]);

        string token = _webEncoder.Base64UrlEncodeGuid();

        var user = _mapper.Map<User>(request);
        user.SetPassword(request.Password);
        user.SetRole("user");
        user.SetVerificationToken(token);

        await _userRepository.AddAsync(user);

        await _mediator.Publish(
            new UserRegisteredDomainEvent(user.FirstName, user.Email, user.VerificationToken));
    }
}