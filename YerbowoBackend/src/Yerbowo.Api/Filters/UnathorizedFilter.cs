namespace Yerbowo.Api.Filters;

public class UnathorizedFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int userIdRequest = Convert.ToInt32(context.ActionArguments["userId"]);
        int userIdHttpContext = Convert.ToInt32(context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (userIdRequest != userIdHttpContext)
        {
            var localizer = context.HttpContext.RequestServices.GetService<IStringLocalizer<SharedResource>>();
            context.Result = new UnauthorizedObjectResult(localizer["Response.Unathorized"]);
        }
    }
}