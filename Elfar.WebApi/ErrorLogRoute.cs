using System.Web.Http;
using System.Web.Http.Routing;
using Elfar.WebApi.RouteConstraints;

namespace Elfar.WebApi
{
    public class ErrorLogRoute
        : HttpRoute
    {
        public ErrorLogRoute(
            IErrorLogProvider provider,
            IHttpRouteConstraint constraint = null)
            : base(Url,
                   new HttpRouteValueDictionary(new { id = RouteParameter.Optional, controller = "ErrorLog" }),
                   new HttpRouteValueDictionary(new { id = new GuidConstraint() }),
                   new HttpRouteValueDictionary(new { provider }))
        {
            if(constraint != null) Constraints.Add(string.Empty, constraint);
        }

        internal const string Url = "api/elfar/{id}";
    }
}