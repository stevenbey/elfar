using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Elfar.Mvc;
using Elfar.Mvc.Resources;
using Elfar.Mvc.Views;

[assembly: PreApplicationStartMethod(typeof(__Package__), "Run")]
namespace Elfar.Mvc
{
    public static class __Package__
    {
        public static void Run()
        {
            ViewEngines.Engines.Insert(0, new Engine());
            GlobalFilters.Filters.Add(new ErrorLogFilter());
            RouteTable.Routes.Insert(0, new ErrorLogRoute());
            HostingEnvironment.RegisterVirtualPathProvider(new Provider());
        }
    }
}