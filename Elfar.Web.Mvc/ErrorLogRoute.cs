using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Elfar.Web.Mvc
{
    // ReSharper disable InconsistentNaming
    public class ErrorLogRoute
    {
        public static implicit operator Route(ErrorLogRoute route)
        {
            return new _Route();
        }

        public static IEnumerable<Routing.IRouteConstraint> Constraints
        {
            get { return constraints ?? (constraints = empty); }
            set { constraints = value; }
        }

        static IEnumerable<Routing.IRouteConstraint> constraints;
        static readonly Routing.IRouteConstraint[] empty = new Routing.IRouteConstraint[0];

        class _Route : Route
        {
            public _Route() : base
            (
                "elfar/{action}/{id}",
                new RouteValueDictionary(new { controller = "ErrorLog", action = "Default", id = UrlParameter.Optional, namespaces = new[] { "Elfar" } }),
                new RouteValueDictionary{ { "", new ErrorLogConstraint() } },
                new _RouteHandler()
            ) { }

            class _RouteHandler : IRouteHandler
            {
                public IHttpHandler GetHttpHandler(RequestContext requestContext)
                {
                    return new ErrorLogController(requestContext);
                }
            }
        }

        class ErrorLogConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                return Constraints.Any(c => c.Match(httpContext));
            }
        }
    }
}