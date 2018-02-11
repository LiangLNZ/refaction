using refactor_me.Models.Services;
using System.Web.Http.Filters;

namespace refactor_me.Api
{
    public class LogExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            ErrorLogService.LogError(context.Exception);
        }
    }
}