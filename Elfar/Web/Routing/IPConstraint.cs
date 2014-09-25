using System.Linq;
using System.Web;

namespace Elfar.Web.Routing
{
    public class IPConstraint : IRouteConstraint
    {
        public IPConstraint(params string[] ips)
        {
            this.ips = ips;
        }

        public bool Match(HttpContextBase httpContext)
        {
            return ips.Contains(httpContext.Request.UserHostAddress);
        }

        readonly string[] ips;
    }
}