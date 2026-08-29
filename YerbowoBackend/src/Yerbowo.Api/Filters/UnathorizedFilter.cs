namespace Yerbowo.Api.Filters;

public class UnathorizedFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Guid userIdRequest = Guid.Parse(context.ActionArguments["userId"].ToString());
        Guid userIdHttpContext = Guid.Parse(context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (userIdRequest != userIdHttpContext)
        {
            var localizer = context.HttpContext.RequestServices.GetService<IStringLocalizer<SharedResource>>();
            context.Result = new UnauthorizedObjectResult(localizer["Response.Unathorized"]);
        }
    }
}