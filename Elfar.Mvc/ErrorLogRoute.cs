using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace Elfar.Mvc
{
    using route = Route;

    public class ErrorLogRoute
    {
        internal class Route : route
        {
            public Route() : base
            (
                "elfar/{action}",
                new RouteValueDictionary { { "action", "Default" }, { "controller", "ErrorLog" }, { "namespaces", new[] { "Elfar" } } },
                new RouteValueDictionary(ErrorLogRoute.Constraints.Where(c => c != null).ToDictionary(k => string.Empty, c => (object) c)),
                new RouteHandler()
            ) {}
        }

        public static implicit operator route(ErrorLogRoute route)
        {
            return new Route();
        }

        public static IEnumerable<IRouteConstraint> Constraints
        {
            get { return constraints ?? (constraints = new IRouteConstraint[0]); }
            set { constraints = value; }
        }

        static IEnumerable<IRouteConstraint> constraints;
    }
}