using System.Linq;
using System.Web;
using System.Web.Routing;
using Elfar.RouteConstraints;

namespace Elfar
{
    public class ErrorLogRoute : Route
    {
        public ErrorLogRoute(IErrorLogProvider provider, IErrorLogPlugin[] plugins)
            : base("elfar/{id}/{action}", new RouteHandler(provider, plugins))
        {
            var constraints = new RouteValueDictionary();

            if(Settings == null) Settings = new ErrorLogRouteSettings();

            foreach(var constraint in Settings.Constraints.Where(constraint => constraint != null))
            {
                constraints.Add(string.Empty, constraint);
            }

            var defaults = new RouteValueDictionary
                {
                    { "namespaces", new[] { "Elfar" } },
                    { "controller", "ErrorLog" }
                };
            
            @default = new Route("elfar/{action}", RouteHandler)
            {
                Defaults = new RouteValueDictionary(defaults)
                {
                    { "action", "Index" }
                },
                Constraints = constraints
            };

            Defaults = new RouteValueDictionary(defaults)
            {
                { "action", "Default" }
            };
            Constraints = new RouteValueDictionary(constraints)
            {
                { "id", new GuidConstraint() }
            };
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            return base.GetRouteData(httpContext)
                ?? @default.GetRouteData(httpContext);
        }
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return base.GetVirtualPath(requestContext, values)
                ?? @default.GetVirtualPath(requestContext, values);
        }

        public static ErrorLogRouteSettings Settings { get; set; }

        readonly Route @default;
    }
}