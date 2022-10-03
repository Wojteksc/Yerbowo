using Yerbowo.Application.Auth.Login;
using Yerbowo.Application.Auth.Register;
using Yerbowo.Application.Auth.SocialLogin;
using Yerbowo.Application.Auth.ConfirmEmail;

namespace Yerbowo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(token);
    }

    [HttpPost("socialLogin")]
    public async Task<IActionResult> Login(SocialLoginCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("confirmEmail")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}