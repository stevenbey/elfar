using System.Web;

namespace Elfar.Web.Routing
{
    public class LocalConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext)
        {
            return httpContext.Request.IsLocal;
        }
    }
}