using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Elfar.WebApi
{
    public class ErrorLogFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (Exclude(actionExecutedContext)) return;

            OnException(actionExecutedContext.Exception, actionExecutedContext.ActionContext.ControllerContext);
        }
        public static void OnException(Exception exception, HttpControllerContext context = null)
        {
            ErrorLogProvider.Save(new ErrorLog(exception, context));
        }

        public static Predicate<HttpActionExecutedContext> Exclude
        {
            get { return exclude ?? (exclude = c => false); }
            set { exclude = value; }
        }

        static Predicate<HttpActionExecutedContext> exclude;
    }
}