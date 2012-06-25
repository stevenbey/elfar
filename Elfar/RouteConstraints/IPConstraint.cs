namespace Elfar.RouteConstraints
{
    using System.Linq;
    using System.Web;
    using System.Web.Routing;

    public class IPConstraint
        : IRouteConstraint
    {
        public IPConstraint(
            params string[] ips)
        {
            this.ips = ips;
        }

        public bool Match(
            HttpContextBase httpContext,
            Route route,
            string parameterName,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            return ips.Contains(httpContext.Request.UserHostAddress);
        }

        readonly string[] ips;
    }
}