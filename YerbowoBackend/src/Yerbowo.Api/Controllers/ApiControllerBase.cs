namespace Yerbowo.Api.Controllers;

[Route("api/[controller]")]
public class ApiControllerBase : ControllerBase
{
    protected int UserId => User?.Identity.IsAuthenticated == true ?
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) :
        0;
        //User.Identity.Name is fetching from --> new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString())  (JwtHandler class)
}