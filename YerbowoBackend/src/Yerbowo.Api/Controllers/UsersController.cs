namespace Yerbowo.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController(IRequestDispatcher dispatcher) : ApiControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDetailsDto>> GetUser(Guid id)
    {
        var user = await dispatcher.ExecuteQuery(new GetUserByIdQuery(id));

        return Ok(user);
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<UserDetailsDto>> GetUser(string email)
    {
        var user = await dispatcher.ExecuteQuery(new GetUserByEmailQuery(email));

        return Ok(user);
    }

    [HttpPut("{id}")]
    [BadRequestFilter]
    public async Task<ActionResult> UpdateUser(Guid id, ChangeUserCommand user)
    {
        await dispatcher.ExecuteCommand(user);

        return NoContent();
    }
}