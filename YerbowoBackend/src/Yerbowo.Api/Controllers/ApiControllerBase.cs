namespace Yerbowo.Api.Controllers;

[Route("api/[controller]")]
public class ApiControllerBase : ControllerBase
{
    protected int UserId => User?.Identity.IsAuthenticated == true 
        ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) 
        : 0;
}