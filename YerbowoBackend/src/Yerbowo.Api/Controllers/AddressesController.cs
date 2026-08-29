namespace Yerbowo.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/users/{userId}/addresses")]
public class AddressesController(IRequestDispatcher dispatcher) : ApiControllerBase
{
    [HttpGet("{id}", Name = nameof(GetAddress))]
    [UnathorizedFilter]
    public async Task<ActionResult<AddressDetailsDto>> GetAddress(Guid userId, Guid id)
    {
        var address = await dispatcher.ExecuteQuery(new GetAddressByIdQuery(id));
        return Ok(address);
    }

    [HttpGet]
    [UnathorizedFilter]
    public async Task<ActionResult<AddressDto>> GetAddresses(Guid userId)
    {
        var addresses = await dispatcher.ExecuteQuery(new GetAddressesByUserIdQuery(userId));
        return Ok(addresses);
    }

    [HttpPost]
    [UnathorizedFilter]
    public async Task<ActionResult<Guid>> Create(Guid userId, CreateAddressCommand command)
    { 
        Guid addressId = await dispatcher.ExecuteCommand(command);

        return CreatedAtRoute(nameof(GetAddress), new { userId, id = addressId }, addressId);
    }

    [HttpPut("{id}")]
    [UnathorizedFilter]
    [BadRequestFilter]
    public async Task<ActionResult> Update(Guid userId, Guid id, ChangeAddressCommand command)
    {
        await dispatcher.ExecuteCommand(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [UnathorizedFilter]
    public async Task<ActionResult> Delete(Guid userId, Guid id)
    {
        await dispatcher.ExecuteCommand(new RemoveAddressCommand(id));

        return NoContent();
    }
}