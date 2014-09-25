using System;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Elfar;

namespace Elfar.Web.Http
{
    class ErrorLog : Elfar.ErrorLog
    {
        public ErrorLog(Exception exception, HttpControllerContext context) : base(exception)
        {
            if(context == null) return;

            var data = context.RouteData;

            if(data != null)
            {
                var route = data.Route as HttpRoute;
                if (route != null)
                {
                    RouteUrl = route.RouteTemplate;
                    DataTokens = route.DataTokens;
                }

                var values = data.Values;

                Action = values["action"].ToPascal();
                Controller = values["controller"].ToPascal();

                RouteData = values;
            }

            var request = context.Request;

            Url = request.RequestUri;
            HttpMethod = request.Method.Method;
        }
    }
}