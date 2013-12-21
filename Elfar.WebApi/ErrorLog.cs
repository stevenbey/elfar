using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Elfar.WebApi
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
                if(route != null) RouteUrl = route.RouteTemplate;

                var values = data.Values;

                Action = ((string) values["action"]).ToTitle();
                Controller = ((string) values["controller"]).ToTitle();

                RouteData = values;
                DataTokens = route.DataTokens;
            }

            var request = context.Request;

            Url = request.RequestUri;
            HttpMethod = request.Method.Method;

            // TODO: the remaining request properties?
        }

        public string Action { get; set; }
        public string Controller { get; set; }
        public IDictionary<string, object> DataTokens { get; set; }
        public string HttpMethod { get; set; }
        public IDictionary<string, object> RouteData { get; set; }
        public string RouteUrl { get; set; }
        public Uri Url { get; set; }
    }
}