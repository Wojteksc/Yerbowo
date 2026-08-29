namespace Yerbowo.Api.Controllers;

[Route("api/[controller]")]
public class ApiControllerBase : ControllerBase
{
    protected Guid UserId => User?.Identity.IsAuthenticated == true 
        ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) 
        : Guid.Empty;
}