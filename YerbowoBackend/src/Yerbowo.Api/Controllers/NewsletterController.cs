namespace Yerbowo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsletterController(IRequestDispatcher dispatcher) : ApiControllerBase
{
    [HttpPost("invite")]
    public async Task<ActionResult<string>> Invite(InviteNewsletterCommand command)
    {
        var message = await dispatcher.ExecuteCommand(command);
        return Ok(message);
    }

    [HttpPost("subscribe")]
    public async Task<ActionResult> Subscribe(SubscribeNewsletterCommand command)
    {
        await dispatcher.ExecuteCommand(command);
        return Ok();
    }

    [HttpPost("unsubscribe")]
    public async Task<ActionResult> Unsubscribe(UnsubscribeNewsletterCommand command)
    {
        await dispatcher.ExecuteCommand(command);
        return Ok();
    }
}