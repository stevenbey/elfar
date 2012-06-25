using System;
using System.Web;
using System.Web.Routing;

namespace Elfar.RouteConstraints
{
    class GuidConstraint
        : IRouteConstraint
    {
        public bool Match(
            HttpContextBase httpContext,
            Route route,
            string parameterName,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            var value = values[parameterName];
            Guid guid;
            return value is Guid || (value is string && Guid.TryParse((string) value, out guid));
        }
    }
}