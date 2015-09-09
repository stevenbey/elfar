using System;
using System.Net.Http;
using System.Web.Http.Routing;

namespace Elfar.Web.Http
{
    sealed class ErrorLog : Elfar.ErrorLog
    {
        public ErrorLog(Exception exception, IHttpRouteData data, HttpRequestMessage request) : base(exception)
        {
            if (data != null)
            {
                var route = data.Route as HttpRoute;
                if (route != null)
                {
                    RouteConstraints = new Dictionary(route.Constraints);
                    DataTokens = new Dictionary(route.DataTokens);
                    RouteDefaults = new Dictionary(route.Defaults);
                    RouteUrl = route.RouteTemplate;
                }

                var values = data.Values;

                Action = values["action"].ToPascal();
                Controller = values["controller"].ToPascal();

                RouteData = new Dictionary(values);
            }

            if (request == null) return;
            
            Url = request.RequestUri;
            HttpMethod = request.Method.Method;
        }
    }
}