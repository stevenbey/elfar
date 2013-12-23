using System.Linq;
using System.Security.Principal;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Elfar.Mvc.Resources;
using Elfar.Mvc.Views;

namespace Elfar.Mvc
{
    public static class Helper
    {
        public static void Init()
        {
            ViewEngines.Engines.Insert(0, new Engine());
            GlobalFilters.Filters.Add(new ErrorLogFilter());
            RouteTable.Routes.Insert(0, new ErrorLogRoute());
            HostingEnvironment.RegisterVirtualPathProvider(new Provider());
        }
        
        internal static bool IsInRoles(this IPrincipal principal, params string[] roles)
        {
            return roles.Any(principal.IsInRole);
        }
    }
}