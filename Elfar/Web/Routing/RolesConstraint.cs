using System.Linq;
using System.Web;

namespace Elfar.Web.Routing
{
    public abstract class RolesConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext)
        {
            return httpContext.Request.IsAuthenticated && Roles.Any(httpContext.User.IsInRole);
        }

        protected abstract string[] Roles { get; }
    }
}