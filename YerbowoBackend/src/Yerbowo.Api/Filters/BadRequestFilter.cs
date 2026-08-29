namespace Yerbowo.Api.Filters;

public class BadRequestFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var idCommand = context.ActionArguments
            .Values
            .Where(v => v is ICommandIdentity)
            .Cast<ICommandIdentity>()
            .Select(x => x.Id)
            .Single();

        Guid idRequest = Guid.Parse(context.ActionArguments["id"].ToString());

        if (idRequest != idCommand) 
        {
            var localizer = context.HttpContext.RequestServices.GetService<IStringLocalizer<SharedResource>>();
            context.Result = new BadRequestObjectResult(localizer["Response.BadRequest"]);
        }
    }
}