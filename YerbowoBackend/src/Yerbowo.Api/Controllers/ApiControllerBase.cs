using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Yerbowo.Api.Controllers
{
    [Route("api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        protected int UserId => User?.Identity.IsAuthenticated == true ?
            //int.Parse(User.Identity.Name) : 
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) :
            0;
        //User.Identity.Name pobierane jest z --> new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString())  (klasa JwtHandler)
    }
}
