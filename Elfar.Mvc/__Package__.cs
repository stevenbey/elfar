using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
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
        static __Package__()
        {
            assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(IsElfarAssembly).ToArray();
            compositionContainer = new CompositionContainer(new AggregateCatalog(assemblies.Select(a => new AssemblyCatalog(a))));
        }

        public static void Run()
        {
            var engine = new Engine(assemblies);
            ViewEngines.Engines.Insert(0, engine);
            VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);

            GlobalFilters.Filters.Add(compositionContainer.GetExport<ErrorLogFilter>().Value);
            RouteTable.Routes.Insert(0, compositionContainer.GetExport<ErrorLogRoute>().Value);
        }

        static bool IsElfarAssembly(Assembly assembly)
        {
            return assembly.FullName.StartsWith("Elfar", StringComparison.OrdinalIgnoreCase);
        }

        static readonly Assembly[] assemblies; 
        static readonly CompositionContainer compositionContainer;
    }
}