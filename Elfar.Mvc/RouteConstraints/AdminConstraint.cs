using System.Web;
using System.Web.Routing;

namespace Elfar.Mvc.RouteConstraints
{
    public class AdminConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return httpContext.Request.IsAuthenticated && (httpContext.User.IsInRoles("Admin", "Administrator", "Administration"));
        }
    }
}