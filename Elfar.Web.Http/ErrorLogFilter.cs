using System;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Http.Routing;

namespace Elfar.Web.Http
{
    public class ErrorLogFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (Exclude(actionExecutedContext)) return;

            OnException(actionExecutedContext.Exception, actionExecutedContext.ActionContext.ControllerContext.RouteData, actionExecutedContext.ActionContext.Request);
        }
        public static void OnException(Exception exception, IHttpRouteData routeData = null, HttpRequestMessage request = null)
        {
            ErrorLogProvider.Save(new ErrorLog(exception, routeData, request));
        }

        public static Predicate<HttpActionExecutedContext> Exclude
        {
            get { return exclude ?? (exclude = c => false); }
            set { exclude = value; }
        }

        static Predicate<HttpActionExecutedContext> exclude;
    }
}