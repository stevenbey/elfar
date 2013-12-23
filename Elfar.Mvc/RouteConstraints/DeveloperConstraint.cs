using System.Web;
using System.Web.Routing;

namespace Elfar.Mvc.RouteConstraints
{
    public class DeveloperConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return httpContext.Request.IsAuthenticated && (httpContext.User.IsInRoles("Dev", "Developer", "Development"));
        }
    }
}