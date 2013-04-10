using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Elfar.Mvc.RouteConstraints;

namespace Elfar.Mvc
{
    public class ErrorLogRoute : Route
    {
        public ErrorLogRoute() : base("elfar/{action}", new RouteHandler())
        {
            var constraints = Settings.Constraints == null
                            ? new Dictionary<string, object>()
                            : Settings.Constraints.Where(c => c != null).ToDictionary(k => string.Empty, c => (object) c);

            var defaults = new RouteValueDictionary
            {
                { "namespaces", new[] { "Elfar" } },
                { "controller", "ErrorLog" }
            };

            withID = new Route("elfar/{id}/{action}", RouteHandler)
            {
                Defaults = new RouteValueDictionary(defaults) { { "action", "Default" } },
                Constraints = new RouteValueDictionary(constraints) { { "id", new GuidConstraint() } }
            };

            Defaults = new RouteValueDictionary(defaults) { { "action", "Index" } };
            Constraints = new RouteValueDictionary(constraints);
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            return withID.GetRouteData(httpContext) ?? base.GetRouteData(httpContext);
        }
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return withID.GetVirtualPath(requestContext, values) ?? base.GetVirtualPath(requestContext, values);
        }

        readonly Route withID;
    }
}