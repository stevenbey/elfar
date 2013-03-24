using System.Web;
using System.Web.Routing;

namespace Elfar
{
    class RouteHandler
        : IRouteHandler
    {
        public RouteHandler(IErrorLogProvider provider, IErrorLogPlugin[] plugins)
        {
            this.provider = provider;
            this.plugins = plugins;
        }
        
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new Handler(requestContext, provider, plugins);
        }

        readonly IErrorLogProvider provider;
        readonly IErrorLogPlugin[] plugins;
    }
}