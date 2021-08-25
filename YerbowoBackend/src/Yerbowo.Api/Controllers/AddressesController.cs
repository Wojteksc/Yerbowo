using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Yerbowo.Application.Addresses.ChangeAddresses;
using Yerbowo.Application.Addresses.CreateAddresses;
using Yerbowo.Application.Addresses.GetAddressDetails;
using Yerbowo.Application.Addresses.GetAddresses;
using Yerbowo.Application.Addresses.RemoveAddresses;

namespace Yerbowo.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users/{userId}/addresses")]
    public class AddressesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AddressesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}", Name = nameof(GetAddress))]
        public async Task<IActionResult> GetAddress(int userId, int id)
        {
            if (userId != UserId)
                return Unauthorized();

            var address = await _mediator.Send(new GetAddressByIdQuery(id));

            return Ok(address);
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses(int userId)
        {
            if (userId != UserId)
                return Unauthorized();

            var addresses = await _mediator.Send(new GetAddressesByUserIdQuery(userId));

            return Ok(addresses);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int userId, CreateAddressCommand command)
        {
            if (userId != UserId)
                return Unauthorized();

            var address = await _mediator.Send(command);

            return CreatedAtRoute(nameof(GetAddress), new { userId, id = address.Id }, address);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int userId, int id, ChangeAddressCommand command)
        {
            if (userId != UserId)
                return Unauthorized();

            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int userId, int id)
        {
            if (userId != UserId)
                return Unauthorized();

            await _mediator.Send(new RemoveAddressComand(id));

            return NoContent();
        }
    }
}