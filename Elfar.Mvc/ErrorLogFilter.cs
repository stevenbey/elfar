using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Elfar.Mvc
{
    public class ErrorLogFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext exceptionContext)
        {
            if (Exclude(exceptionContext)) return;

            OnException(exceptionContext.Exception, exceptionContext.RouteData, exceptionContext.HttpContext);
        }
        public static void OnException(Exception exception, RouteData routeData, HttpContextBase context)
        {
            var errorLog = new ErrorLog(Application, exception, routeData, context);

            ErrorLogPlugins.Execute(errorLog);

            if(exception is ErrorLogException) return;

            ErrorLogProvider.Save(errorLog);
        }

        public static Predicate<ExceptionContext> Exclude
        {
            get { return exclude ?? (exclude = (c => false)); }
            set { exclude = value; }
        }

        static string Application
        {
            get { return ErrorLogProvider.Settings.Application; }
        }

        static Predicate<ExceptionContext> exclude;
    }
}