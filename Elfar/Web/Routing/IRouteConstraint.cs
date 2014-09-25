using System.Web;

namespace Elfar.Web.Routing
{
    public interface IRouteConstraint
    {
        bool Match(HttpContextBase httpContext);
    }
}