namespace Yerbowo.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController(IRequestDispatcher dispatcher) : ApiControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDetailsDto>> GetUser(int id)
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
    public async Task<ActionResult> UpdateUser(int id, ChangeUserCommand user)
    {
        await dispatcher.ExecuteCommand(user);

        return NoContent();
    }
}