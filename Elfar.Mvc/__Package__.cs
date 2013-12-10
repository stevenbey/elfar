using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using Elfar.Mvc;
using Elfar.Mvc.Views;

[assembly: PreApplicationStartMethod(typeof(__Package__), "Run")]
namespace Elfar.Mvc
{
    public static class __Package__
    {
        public static void Run()
        {
            var engine = new Engine(Components.Assemblies);
            ViewEngines.Engines.Insert(0, engine);
            VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);

            GlobalFilters.Filters.Add(new ErrorLogFilter());
            RouteTable.Routes.Insert(0, new ErrorLogRoute());
        }
    }
}