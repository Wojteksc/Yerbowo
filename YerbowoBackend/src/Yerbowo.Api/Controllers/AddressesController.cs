namespace Yerbowo.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/users/{userId}/addresses")]
public class AddressesController(IRequestDispatcher dispatcher) : ApiControllerBase
{
    [HttpGet("{id}", Name = nameof(GetAddress))]
    [UnathorizedFilter]
    public async Task<ActionResult<AddressDetailsDto>> GetAddress(int userId, int id)
    {
        var address = await dispatcher.ExecuteQuery(new GetAddressByIdQuery(id));
        return Ok(address);
    }

    [HttpGet]
    [UnathorizedFilter]
    public async Task<ActionResult<AddressDto>> GetAddresses(int userId)
    {
        var addresses = await dispatcher.ExecuteQuery(new GetAddressesByUserIdQuery(userId));
        return Ok(addresses);
    }

    [HttpPost]
    [UnathorizedFilter]
    public async Task<ActionResult<int>> Create(int userId, CreateAddressCommand command)
    { 
        int addressId = await dispatcher.ExecuteCommand(command);

        return CreatedAtRoute(nameof(GetAddress), new { userId, id = addressId }, addressId);
    }

    [HttpPut("{id}")]
    [UnathorizedFilter]
    [BadRequestFilter]
    public async Task<ActionResult> Update(int userId, int id, ChangeAddressCommand command)
    {
        await dispatcher.ExecuteCommand(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [UnathorizedFilter]
    public async Task<ActionResult> Delete(int userId, int id)
    {
        await dispatcher.ExecuteCommand(new RemoveAddressCommand(id));

        return NoContent();
    }
}