using System.Web.Routing;
using System.Web;

namespace Elfar.Mvc.RouteConstraints
{
    public class LocalConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return httpContext.Request.IsLocal;
        }
    }
}