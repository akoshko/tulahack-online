using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tulahack.API.Extensions;

namespace Tulahack.API.Utils;

public class UserIdActionFilterAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        var userId = context.HttpContext.User.GetUserId();
        if (userId == default)
            context.Result = new UnprocessableEntityResult();
    }
}