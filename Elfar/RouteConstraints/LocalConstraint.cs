namespace Elfar.RouteConstraints
{
    using System.Web.Routing;
    using System.Web;

    public class LocalConstraint
        : IRouteConstraint
    {
        public bool Match(
            HttpContextBase httpContext,
            Route route,
            string parameterName,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            return httpContext.Request.IsLocal;
        }
    }
}