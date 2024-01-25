namespace Yerbowo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsletterController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public NewsletterController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("invite")]
    public async Task<IActionResult> Invite(InviteNewsletterCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe(SubscribeNewsletterCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("unsubscribe")]
    public async Task<IActionResult> Unsubscribe(UnsubscribeNewsletterCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}