namespace Yerbowo.Application.Functions.Auth.Command.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public RegisterHandler(IUserRepository userRepository,
        IMapper mapper,
        IMediator mediator,
        IStringLocalizer<SharedResource> localizer)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _mediator = mediator;
        _localizer = localizer;
    }

    public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsAsync(request.Email))
            throw new Exception(_localizer["ExceptionEmailIsAlreadyInUse"]);

        string token = WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray());

        var user = _mapper.Map<User>(request);
        user.SetPassword(request.Password);
        user.SetRole("user");
        user.SetVerificationToken(token);

        await _userRepository.AddAsync(user);

        await _mediator.Publish(new RegisterEndedEvent(user));

        return Unit.Value;
    }
}