using System.Web;
using System.Web.Routing;

namespace Elfar
{
    class RouteHandler
        : IRouteHandler
    {
        public RouteHandler(
            IErrorLogProvider provider)
        {
            this.provider = provider;
        }

        public IHttpHandler GetHttpHandler(
            RequestContext requestContext)
        {
            return new Handler(requestContext, provider);
        }

        readonly IErrorLogProvider provider;
    }
}