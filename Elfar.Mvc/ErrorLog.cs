using System;
using System.Web;
using System.Web.Routing;

namespace Elfar.Mvc
{
    class ErrorLog : Elfar.ErrorLog
    {
        public ErrorLog(string application, Exception exception, RouteData routeData, HttpContextBase context) : base(application, new Json(exception, routeData, context)) {}
    }
}