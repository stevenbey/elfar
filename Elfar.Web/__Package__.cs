using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Elfar.Twitter;
using Elfar.Xml;

[assembly: PreApplicationStartMethod(typeof(Elfar.Web.__Package__), "Run")]
namespace Elfar.Web
{
    public static class __Package__
    {
        public static void Run()
        {
            var provider = new XmlErrorLogProvider();
            GlobalFilters.Filters.Add(new ErrorLogFilter(provider, tweet: new ErrorLogTweet(null, null)));
            RouteTable.Routes.Insert(0, new ErrorLogRoute(provider));

            UpdateViewEngines(ViewEngines.Engines);

            RouteTable.Routes.MapRoute("", "", new { controller = "Default", action = "Default" });
        }

        static void UpdateViewEngines(ICollection<IViewEngine> engines)
        {
            var engine = engines.FirstOrDefault(e => e is WebFormViewEngine);
            if(engine != null) engines.Remove(engine);
        }
    }
}