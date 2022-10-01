using Microsoft.AspNetCore.WebUtilities;
using Yerbowo.Application.Auth.Register.Events;
using Yerbowo.Domain.Users;
using Yerbowo.Infrastructure.Context;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Application.Auth.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;
    private readonly YerbowoContext _yerbowoContext;

    public RegisterHandler(IUserRepository userRepository,
        IMapper mapper,
        IMediator mediator,
        YerbowoContext yerbowoContext)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _mediator = mediator;
        _yerbowoContext = yerbowoContext;
    }

    public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsAsync(request.Email))
            throw new Exception("Ten adres e-mail jest już używany, proszę wybierz inny albo zaloguj się.");

        string token = WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray());

        var user = _mapper.Map<User>(request);
        user.SetPassword(request.Password);
        user.SetRole("user");
        user.SetVerificationToken(token);

        using (var dbTransaction = await _yerbowoContext.Database.BeginTransactionAsync())
        {
            try
            {
                await _userRepository.AddAsync(user);
                await _mediator.Publish(new RegisterEndedEvent(user));
                await dbTransaction.CommitAsync();
            }
            catch (Exception)
            {
                await dbTransaction.RollbackAsync();
                throw new Exception("Nieudana próba rejestracji konta. Skontaktuj się z administratorem.");
            }
        }
        return Unit.Value;
    }
}