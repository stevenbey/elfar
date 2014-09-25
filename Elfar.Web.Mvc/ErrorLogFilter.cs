using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Elfar.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ErrorLogFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext exceptionContext)
        {
            if (Exclude(exceptionContext)) return;

            OnException(exceptionContext.Exception, exceptionContext.RouteData, exceptionContext.HttpContext);
        }
        public static void OnException(Exception exception, RouteData routeData = null, HttpContextBase context = null)
        {
            ErrorLogProvider.Save(new ErrorLog(exception, routeData, context));
        }

        public static Predicate<ExceptionContext> Exclude
        {
            get { return exclude ?? (exclude = c => false); }
            set { exclude = value; }
        }
        
        static Predicate<ExceptionContext> exclude;
    }
}