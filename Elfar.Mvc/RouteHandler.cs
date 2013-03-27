using System.ComponentModel.Composition;
using System.Web;
using System.Web.Routing;

namespace Elfar.Mvc
{
    [Export]
    public class RouteHandler : IRouteHandler
    {
        [ImportingConstructor]
        public RouteHandler(IErrorLogProvider provider)
        {
            this.provider = provider;
        }
        
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new Handler(requestContext, provider, Plugins);
        }
        
        [ImportMany]
        public IErrorLogPlugin[] Plugins;

        readonly IErrorLogProvider provider;
    }
}