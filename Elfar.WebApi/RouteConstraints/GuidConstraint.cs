using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Elfar.WebApi.RouteConstraints
{
    class GuidConstraint
        : IHttpRouteConstraint
    {
        public bool Match(
            HttpRequestMessage request,
            IHttpRoute route,
            string parameterName,
            IDictionary<string, object> values,
            HttpRouteDirection routeDirection)
        {
            var value = values[parameterName];
            Guid guid;
            return value is RouteParameter || value is Guid || (value is string && Guid.TryParse((string)value, out guid));
        }
    }
}