namespace Yerbowo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IRequestDispatcher dispatcher) : ApiControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<ResponseToken>> Login(LoginCommand command)
    {
        var token = await dispatcher.ExecuteCommand(command);
        return Ok(token);
    }

    [HttpPost("socialLogin")]
    public async Task<ActionResult<ResponseToken>> Login(SocialLoginCommand command)
    {
        var token = await dispatcher.ExecuteCommand(command);
        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterCommand command)
    {
        await dispatcher.ExecuteCommand(command);
        return Ok();
    }

    [HttpPost("confirmEmail")]
    public async Task<ActionResult> ConfirmEmail(ConfirmRegistrationEmailCommand command)
    {
        await dispatcher.ExecuteCommand(command);
        return Ok();
    }
}