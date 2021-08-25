using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Yerbowo.Application.Users.ChangeUsers;
using Yerbowo.Application.Users.GetUserDetails;

namespace Yerbowo.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));

            return Ok(user);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            var user = await _mediator.Send(new GetUserByEmailQuery(email));

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, ChangeUserCommand user)
        {
            if (id != user.Id)
                return BadRequest();

            await _mediator.Send(user);

            return NoContent();
        }
    }
}