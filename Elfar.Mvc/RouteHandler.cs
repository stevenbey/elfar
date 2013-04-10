using System.Web;
using System.Web.Routing;

namespace Elfar.Mvc
{
    public class RouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new Handler(requestContext);
        }
    }
}